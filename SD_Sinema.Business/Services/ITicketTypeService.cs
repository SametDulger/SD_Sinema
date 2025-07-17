using SD_Sinema.Business.DTOs;

namespace SD_Sinema.Business.Services
{
    public interface ITicketTypeService
    {
        Task<IEnumerable<TicketTypeDto>> GetAllAsync();
        Task<TicketTypeDto?> GetByIdAsync(int id);
        Task<TicketTypeDto> CreateAsync(CreateTicketTypeDto createTicketTypeDto);
        Task<TicketTypeDto> UpdateAsync(int id, UpdateTicketTypeDto updateTicketTypeDto);
        Task DeleteAsync(int id, string deletedBy, string reason);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<TicketTypeDto>> GetActiveTicketTypesAsync();
    }
} 