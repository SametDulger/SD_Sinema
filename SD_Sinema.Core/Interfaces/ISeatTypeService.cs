using SD_Sinema.Core.Entities;

namespace SD_Sinema.Core.Interfaces
{
    public interface ISeatTypeService
    {
        Task<IEnumerable<SeatType>> GetAllAsync();
        Task<IEnumerable<SeatType>> GetActiveSeatTypesAsync();
        Task<IEnumerable<string>> GetActiveSeatTypeNamesAsync();
        Task<SeatType?> GetByIdAsync(int id);
        Task<SeatType> CreateAsync(SeatType seatType);
        Task<SeatType> UpdateAsync(int id, SeatType seatType);
        Task DeleteAsync(int id);
    }
} 