using SD_Sinema.Business.DTOs;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;

namespace SD_Sinema.Business.Services
{
    public class SalonService : ISalonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SalonDto>> GetAllAsync()
        {
            var salons = await _unitOfWork.Salons.GetAllAsync();
            return salons.Select(MapToDto);
        }

        public async Task<SalonDto?> GetByIdAsync(int id)
        {
            var salon = await _unitOfWork.Salons.GetByIdAsync(id);
            return salon != null ? MapToDto(salon) : null;
        }

        public async Task<SalonDto> CreateAsync(CreateSalonDto createSalonDto)
        {
            var salon = new Salon
            {
                Name = createSalonDto.Name,
                Capacity = createSalonDto.Capacity,
                Description = createSalonDto.Description
            };

            await _unitOfWork.Salons.AddAsync(salon);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(salon);
        }

        public async Task<SalonDto> UpdateAsync(int id, UpdateSalonDto updateSalonDto)
        {
            var salon = await _unitOfWork.Salons.GetByIdAsync(id);
            if (salon == null)
                throw new InvalidOperationException("Salon bulunamadı.");

            salon.Name = updateSalonDto.Name;
            salon.Capacity = updateSalonDto.Capacity;
            salon.Description = updateSalonDto.Description;
            salon.IsActive = updateSalonDto.IsActive;

            await _unitOfWork.Salons.UpdateAsync(salon);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(salon);
        }

        public async Task DeleteAsync(int id, string deletedBy, string reason)
        {
            await _unitOfWork.Salons.DeleteAsync(id, deletedBy, reason);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Salons.ExistsAsync(id);
        }

        public async Task<IEnumerable<SalonDto>> GetActiveSalonsAsync()
        {
            var salons = await _unitOfWork.Salons.GetAllAsync();
            return salons.Where(s => s.IsActive).Select(MapToDto);
        }

        private static SalonDto MapToDto(Salon salon)
        {
            return new SalonDto
            {
                Id = salon.Id,
                Name = salon.Name,
                Capacity = salon.Capacity,
                Description = salon.Description,
                IsActive = salon.IsActive,
                CreatedDate = salon.CreatedDate
            };
        }
    }
} 