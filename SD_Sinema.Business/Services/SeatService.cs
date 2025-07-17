using SD_Sinema.Business.DTOs;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;

namespace SD_Sinema.Business.Services
{
    public class SeatService : ISeatService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SeatDto>> GetAllAsync()
        {
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return seats.Select(s => MapToDto(s, salons));
        }

        public async Task<SeatDto?> GetByIdAsync(int id)
        {
            var seat = await _unitOfWork.Seats.GetByIdAsync(id);
            if (seat == null) return null;

            var salons = await _unitOfWork.Salons.GetAllAsync();

            return MapToDto(seat, salons);
        }

        public async Task<SeatDto> CreateAsync(CreateSeatDto createSeatDto)
        {
            var seat = new Seat
            {
                SalonId = createSeatDto.SalonId,
                RowNumber = createSeatDto.RowNumber.ToString(),
                SeatNumber = int.Parse(createSeatDto.SeatNumber)
            };

            await _unitOfWork.Seats.AddAsync(seat);
            await _unitOfWork.SaveChangesAsync();

            var salons = await _unitOfWork.Salons.GetAllAsync();

            return MapToDto(seat, salons);
        }

        public async Task<SeatDto> UpdateAsync(int id, UpdateSeatDto updateSeatDto)
        {
            var seat = await _unitOfWork.Seats.GetByIdAsync(id);
            if (seat == null)
                throw new InvalidOperationException("Koltuk bulunamadı.");

            seat.SalonId = updateSeatDto.SalonId;
            seat.RowNumber = updateSeatDto.RowNumber.ToString();
            seat.SeatNumber = int.Parse(updateSeatDto.SeatNumber);
            seat.IsActive = true; // Varsayılan değer

            await _unitOfWork.Seats.UpdateAsync(seat);
            await _unitOfWork.SaveChangesAsync();

            var salons = await _unitOfWork.Salons.GetAllAsync();

            return MapToDto(seat, salons);
        }

        public async Task DeleteAsync(int id, string deletedBy, string reason)
        {
            await _unitOfWork.Seats.DeleteAsync(id, deletedBy, reason);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Seats.ExistsAsync(id);
        }

        public async Task<IEnumerable<SeatDto>> GetSeatsBySalonAsync(int salonId)
        {
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return seats.Where(s => s.SalonId == salonId && s.IsActive)
                       .Select(s => MapToDto(s, salons));
        }

        public async Task<IEnumerable<SeatDto>> GetActiveSeatsAsync()
        {
            var seats = await _unitOfWork.Seats.GetAllAsync();
            var salons = await _unitOfWork.Salons.GetAllAsync();

            return seats.Where(s => s.IsActive).Select(s => MapToDto(s, salons));
        }

        private static SeatDto MapToDto(Seat seat, IEnumerable<Salon> salons)
        {
            var salon = salons.FirstOrDefault(s => s.Id == seat.SalonId);

            return new SeatDto
            {
                Id = seat.Id,
                SalonId = seat.SalonId,
                RowNumber = seat.RowNumber,
                SeatNumber = seat.SeatNumber,
                IsActive = seat.IsActive,
                SalonName = salon?.Name ?? "",
                CreatedDate = seat.CreatedDate
            };
        }
    }
} 