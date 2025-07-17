using SD_Sinema.Business.DTOs;

namespace SD_Sinema.Business.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllAsync();
        Task<TicketDto?> GetByIdAsync(int id);
        Task<TicketDto> CreateAsync(CreateTicketDto createTicketDto);
        Task<TicketDto> UpdateAsync(int id, UpdateTicketDto updateTicketDto);
        Task DeleteAsync(int id, string deletedBy, string reason);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<TicketDto>> GetTicketsByUserAsync(int userId);
        Task<IEnumerable<TicketDto>> GetTicketsBySessionAsync(int sessionId);
        Task<decimal> CalculateTicketPriceAsync(int ticketTypeId, int userId);
    }
} 