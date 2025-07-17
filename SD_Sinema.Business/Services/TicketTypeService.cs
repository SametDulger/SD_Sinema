using SD_Sinema.Business.DTOs;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;

namespace SD_Sinema.Business.Services
{
    public class TicketTypeService : ITicketTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TicketTypeDto>> GetAllAsync()
        {
            var ticketTypes = await _unitOfWork.TicketTypes.GetAllAsync();
            return ticketTypes.Select(MapToDto);
        }

        public async Task<TicketTypeDto?> GetByIdAsync(int id)
        {
            var ticketType = await _unitOfWork.TicketTypes.GetByIdAsync(id);
            return ticketType != null ? MapToDto(ticketType) : null;
        }

        public async Task<TicketTypeDto> CreateAsync(CreateTicketTypeDto createTicketTypeDto)
        {
            var ticketType = new TicketType
            {
                Name = createTicketTypeDto.Name,
                Description = createTicketTypeDto.Description,
                Price = createTicketTypeDto.Price,
                DiscountPercentage = createTicketTypeDto.DiscountPercentage
            };

            await _unitOfWork.TicketTypes.AddAsync(ticketType);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(ticketType);
        }

        public async Task<TicketTypeDto> UpdateAsync(int id, UpdateTicketTypeDto updateTicketTypeDto)
        {
            var ticketType = await _unitOfWork.TicketTypes.GetByIdAsync(id);
            if (ticketType == null)
                throw new InvalidOperationException("Bilet tipi bulunamadı.");

            ticketType.Name = updateTicketTypeDto.Name;
            ticketType.Description = updateTicketTypeDto.Description;
            ticketType.Price = updateTicketTypeDto.Price;
            ticketType.DiscountPercentage = updateTicketTypeDto.DiscountPercentage;
            ticketType.IsActive = updateTicketTypeDto.IsActive;

            await _unitOfWork.TicketTypes.UpdateAsync(ticketType);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(ticketType);
        }

        public async Task DeleteAsync(int id, string deletedBy, string reason)
        {
            await _unitOfWork.TicketTypes.DeleteAsync(id, deletedBy, reason);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.TicketTypes.ExistsAsync(id);
        }

        public async Task<IEnumerable<TicketTypeDto>> GetActiveTicketTypesAsync()
        {
            var ticketTypes = await _unitOfWork.TicketTypes.GetAllAsync();
            return ticketTypes.Where(t => t.IsActive).Select(MapToDto);
        }

        private static TicketTypeDto MapToDto(TicketType ticketType)
        {
            return new TicketTypeDto
            {
                Id = ticketType.Id,
                Name = ticketType.Name,
                Description = ticketType.Description,
                Price = ticketType.Price,
                DiscountPercentage = ticketType.DiscountPercentage,
                IsActive = ticketType.IsActive,
                CreatedDate = ticketType.CreatedDate
            };
        }
    }
} 