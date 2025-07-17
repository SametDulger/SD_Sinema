using SD_Sinema.Business.DTOs;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;

namespace SD_Sinema.Business.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReservationDto>> GetAllAsync()
        {
            var reservations = await _unitOfWork.Reservations.GetAllAsync();
            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return reservations.Select(r => MapToDto(r, users, sessions, seats, movies, salons));
        }

        public async Task<ReservationDto?> GetByIdAsync(int id)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);
            if (reservation == null) return null;

            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return MapToDto(reservation, users, sessions, seats, movies, salons);
        }

        public async Task<ReservationDto> CreateAsync(CreateReservationDto createReservationDto)
        {
            var reservation = new Reservation
            {
                UserId = createReservationDto.UserId,
                SessionId = createReservationDto.SessionId,
                SeatId = createReservationDto.SeatId,
                ReservationDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddHours(2),
                Status = "Pending",
                Notes = createReservationDto.Notes,
                Price = createReservationDto.Price
            };

            await _unitOfWork.Reservations.AddAsync(reservation);
            await _unitOfWork.SaveChangesAsync();

            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return MapToDto(reservation, users, sessions, seats, movies, salons);
        }

        public async Task<ReservationDto> UpdateAsync(int id, UpdateReservationDto updateReservationDto)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);
            if (reservation == null)
                throw new InvalidOperationException("Rezervasyon bulunamadı.");

            reservation.Status = updateReservationDto.Status;
            reservation.Notes = updateReservationDto.Notes;
            reservation.Price = updateReservationDto.Price;

            await _unitOfWork.Reservations.UpdateAsync(reservation);
            await _unitOfWork.SaveChangesAsync();

            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return MapToDto(reservation, users, sessions, seats, movies, salons);
        }

        public async Task DeleteAsync(int id, string deletedBy, string reason)
        {
            await _unitOfWork.Reservations.DeleteAsync(id, deletedBy, reason);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Reservations.ExistsAsync(id);
        }

        public async Task<IEnumerable<ReservationDto>> GetReservationsByUserAsync(int userId)
        {
            var reservations = await _unitOfWork.Reservations.GetAllAsync();
            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return reservations.Where(r => r.UserId == userId)
                              .Select(r => MapToDto(r, users, sessions, seats, movies, salons));
        }

        public async Task<IEnumerable<ReservationDto>> GetReservationsBySessionAsync(int sessionId)
        {
            var reservations = await _unitOfWork.Reservations.GetAllAsync();
            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return reservations.Where(r => r.SessionId == sessionId)
                              .Select(r => MapToDto(r, users, sessions, seats, movies, salons));
        }

        public async Task<ReservationDto> ApproveReservationAsync(int id)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);
            if (reservation == null)
                throw new InvalidOperationException("Rezervasyon bulunamadı.");

            reservation.Status = "Approved";
            reservation.ExpiryDate = DateTime.Now.AddDays(1);

            await _unitOfWork.Reservations.UpdateAsync(reservation);
            await _unitOfWork.SaveChangesAsync();

            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return MapToDto(reservation, users, sessions, seats, movies, salons);
        }

        public async Task<ReservationDto> CancelReservationAsync(int id)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);
            if (reservation == null)
                throw new InvalidOperationException("Rezervasyon bulunamadı.");

            reservation.Status = "Cancelled";

            await _unitOfWork.Reservations.UpdateAsync(reservation);
            await _unitOfWork.SaveChangesAsync();

            var users = await _unitOfWork.Users.GetAllAsync();
            var sessions = await _unitOfWork.Sessions.GetAllAsync();
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return MapToDto(reservation, users, sessions, seats, movies, salons);
        }

        private static ReservationDto MapToDto(Reservation reservation, IEnumerable<User> users, IEnumerable<Session> sessions, 
            IEnumerable<Seat> seats, IEnumerable<Movie> movies, IEnumerable<Salon> salons)
        {
            var user = users.FirstOrDefault(u => u.Id == reservation.UserId);
            var session = sessions.FirstOrDefault(s => s.Id == reservation.SessionId);
            var seat = seats.FirstOrDefault(s => s.Id == reservation.SeatId);
            var movie = movies.FirstOrDefault(m => m.Id == session?.MovieId);
            var salon = salons.FirstOrDefault(s => s.Id == session?.SalonId);

            return new ReservationDto
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                SessionId = reservation.SessionId,
                SeatId = reservation.SeatId,
                ReservationDate = reservation.ReservationDate,
                ExpiryDate = reservation.ExpiryDate,
                Status = reservation.Status,
                Notes = reservation.Notes,
                Price = reservation.Price,
                UserName = $"{user?.FirstName} {user?.LastName}",
                MovieTitle = movie?.Title ?? "",
                SalonName = salon?.Name ?? "",
                SeatInfo = $"{seat?.RowNumber}{seat?.SeatNumber}",
                CreatedDate = reservation.CreatedDate
            };
        }
    }
} 