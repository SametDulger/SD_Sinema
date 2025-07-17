using SD_Sinema.Business.DTOs;

namespace SD_Sinema.Business.Services
{
    public interface ISeatService
    {
        Task<IEnumerable<SeatDto>> GetAllAsync();
        Task<SeatDto?> GetByIdAsync(int id);
        Task<SeatDto> CreateAsync(CreateSeatDto createSeatDto);
        Task<SeatDto> UpdateAsync(int id, UpdateSeatDto updateSeatDto);
        Task DeleteAsync(int id, string deletedBy, string reason);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<SeatDto>> GetSeatsBySalonAsync(int salonId);
        Task<IEnumerable<SeatDto>> GetActiveSeatsAsync();
    }
} 