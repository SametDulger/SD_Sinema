using SD_Sinema.Business.DTOs;

namespace SD_Sinema.Business.Services
{
    public interface ISalonService
    {
        Task<IEnumerable<SalonDto>> GetAllAsync();
        Task<SalonDto?> GetByIdAsync(int id);
        Task<SalonDto> CreateAsync(CreateSalonDto createSalonDto);
        Task<SalonDto> UpdateAsync(int id, UpdateSalonDto updateSalonDto);
        Task DeleteAsync(int id, string deletedBy, string reason);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<SalonDto>> GetActiveSalonsAsync();
    }
} 