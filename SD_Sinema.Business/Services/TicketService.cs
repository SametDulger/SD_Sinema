using SD_Sinema.Business.DTOs;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;

namespace SD_Sinema.Business.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var tickets = await _unitOfWork.Tickets.GetAllAsync();
            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var ticketTypes = await _unitOfWork.TicketTypes.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return tickets.Select(t => MapToDto(t, users, sessions, seats, ticketTypes, movies, salons));
        }

        public async Task<TicketDto?> GetByIdAsync(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null) return null;

            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var ticketTypes = await _unitOfWork.TicketTypes.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return MapToDto(ticket, users, sessions, seats, ticketTypes, movies, salons);
        }

        public async Task<TicketDto> CreateAsync(CreateTicketDto createTicketDto)
        {
            var price = await CalculateTicketPriceAsync(createTicketDto.TicketTypeId, createTicketDto.UserId);
            var user = await _unitOfWork.Users.GetByIdAsync(createTicketDto.UserId);
            var isVipDiscount = user?.IsVipMember == true;
            var isFreeMovie = false;

            if (isVipDiscount && user?.UsedFreeMovieCount < user?.MonthlyFreeMovieCount)
            {
                isFreeMovie = true;
                price = 0;
                user.UsedFreeMovieCount++;
                await _unitOfWork.Users.UpdateAsync(user);
            }

            var ticket = new Ticket
            {
                UserId = createTicketDto.UserId,
                SessionId = createTicketDto.SessionId,
                SeatId = createTicketDto.SeatId,
                TicketTypeId = createTicketDto.TicketTypeId,
                PurchaseDate = DateTime.Now,
                Price = price,
                PaymentMethod = createTicketDto.PaymentMethod,
                TransactionId = createTicketDto.TransactionId,
                IsVipDiscount = isVipDiscount,
                IsFreeMovie = isFreeMovie
            };

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var ticketTypes = await _unitOfWork.TicketTypes.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return MapToDto(ticket, users, sessions, seats, ticketTypes, movies, salons);
        }

        public async Task<TicketDto> UpdateAsync(int id, UpdateTicketDto updateTicketDto)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null)
                throw new InvalidOperationException("Bilet bulunamadı.");

            ticket.Status = updateTicketDto.Status;
            ticket.PaymentMethod = updateTicketDto.PaymentMethod;
            ticket.TransactionId = updateTicketDto.TransactionId;

            await _unitOfWork.Tickets.UpdateAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var ticketTypes = await _unitOfWork.TicketTypes.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return MapToDto(ticket, users, sessions, seats, ticketTypes, movies, salons);
        }

        public async Task DeleteAsync(int id, string deletedBy, string reason)
        {
            await _unitOfWork.Tickets.DeleteAsync(id, deletedBy, reason);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Tickets.ExistsAsync(id);
        }

        public async Task<IEnumerable<TicketDto>> GetTicketsByUserAsync(int userId)
        {
            var tickets = await _unitOfWork.Tickets.GetAllAsync();
            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var ticketTypes = await _unitOfWork.TicketTypes.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return tickets.Where(t => t.UserId == userId)
                         .Select(t => MapToDto(t, users, sessions, seats, ticketTypes, movies, salons));
        }

        public async Task<IEnumerable<TicketDto>> GetTicketsBySessionAsync(int sessionId)
        {
            var tickets = await _unitOfWork.Tickets.GetAllAsync();
            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var ticketTypes = await _unitOfWork.TicketTypes.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return tickets.Where(t => t.SessionId == sessionId)
                         .Select(t => MapToDto(t, users, sessions, seats, ticketTypes, movies, salons));
        }

        public async Task<decimal> CalculateTicketPriceAsync(int ticketTypeId, int userId)
        {
            var ticketType = await _unitOfWork.TicketTypes.GetByIdAsync(ticketTypeId);
            if (ticketType == null)
                throw new InvalidOperationException("Bilet tipi bulunamadı.");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            var basePrice = ticketType.Price;

            if (user?.IsVipMember == true && ticketType.DiscountPercentage.HasValue)
            {
                var discount = basePrice * (ticketType.DiscountPercentage.Value / 100);
                basePrice -= discount;
            }

            return Math.Round(basePrice, 2);
        }

        private static TicketDto MapToDto(Ticket ticket, IEnumerable<User> users, IEnumerable<Session> sessions, 
            IEnumerable<Seat> seats, IEnumerable<TicketType> ticketTypes, IEnumerable<Movie> movies, IEnumerable<Salon> salons)
        {
            var user = users.FirstOrDefault(u => u.Id == ticket.UserId);
            var session = sessions.FirstOrDefault(s => s.Id == ticket.SessionId);
            var seat = seats.FirstOrDefault(s => s.Id == ticket.SeatId);
            var ticketType = ticketTypes.FirstOrDefault(t => t.Id == ticket.TicketTypeId);
            var movie = movies.FirstOrDefault(m => m.Id == session?.MovieId);
            var salon = salons.FirstOrDefault(s => s.Id == session?.SalonId);

            return new TicketDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                SessionId = ticket.SessionId,
                SeatId = ticket.SeatId,
                TicketTypeId = ticket.TicketTypeId,
                PurchaseDate = ticket.PurchaseDate,
                Price = ticket.Price,
                Status = ticket.Status,
                PaymentMethod = ticket.PaymentMethod,
                TransactionId = ticket.TransactionId,
                IsVipDiscount = ticket.IsVipDiscount,
                IsFreeMovie = ticket.IsFreeMovie,
                UserName = $"{user?.FirstName} {user?.LastName}",
                MovieTitle = movie?.Title ?? "",
                SalonName = salon?.Name ?? "",
                SeatInfo = $"{seat?.RowNumber}{seat?.SeatNumber}",
                TicketTypeName = ticketType?.Name ?? "",
                CreatedDate = ticket.CreatedDate
            };
        }
    }
} 