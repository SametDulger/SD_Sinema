using SD_Sinema.Business.DTOs;

namespace SD_Sinema.Business.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDto>> GetAllAsync();
        Task<ReservationDto?> GetByIdAsync(int id);
        Task<ReservationDto> CreateAsync(CreateReservationDto createReservationDto);
        Task<ReservationDto> UpdateAsync(int id, UpdateReservationDto updateReservationDto);
        Task DeleteAsync(int id, string deletedBy, string reason);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<ReservationDto>> GetReservationsByUserAsync(int userId);
        Task<IEnumerable<ReservationDto>> GetReservationsBySessionAsync(int sessionId);
        Task<ReservationDto> ApproveReservationAsync(int id);
        Task<ReservationDto> CancelReservationAsync(int id);
    }
} 