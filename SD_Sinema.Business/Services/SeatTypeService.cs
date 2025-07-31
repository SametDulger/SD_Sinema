using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Repositories;

namespace SD_Sinema.Business.Services
{
    public class SeatTypeService : ISeatTypeService
    {
        private readonly IRepository<SeatType> _seatTypeRepository;

        public SeatTypeService(IRepository<SeatType> seatTypeRepository)
        {
            _seatTypeRepository = seatTypeRepository;
        }

        public async Task<IEnumerable<SeatType>> GetAllAsync()
        {
            return await _seatTypeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<SeatType>> GetActiveSeatTypesAsync()
        {
            return await _seatTypeRepository.GetAllAsync(st => st.IsActive);
        }

        public async Task<IEnumerable<string>> GetActiveSeatTypeNamesAsync()
        {
            var seatTypes = await _seatTypeRepository.GetAllAsync(st => st.IsActive);
            return seatTypes.Select(st => st.Name);
        }

        public async Task<SeatType?> GetByIdAsync(int id)
        {
            return await _seatTypeRepository.GetByIdAsync(id);
        }

        public async Task<SeatType> CreateAsync(SeatType seatType)
        {
            return await _seatTypeRepository.AddAsync(seatType);
        }

        public async Task<SeatType> UpdateAsync(int id, SeatType seatType)
        {
            var existingSeatType = await _seatTypeRepository.GetByIdAsync(id);
            if (existingSeatType == null)
                throw new InvalidOperationException("SeatType bulunamadı.");

            existingSeatType.Name = seatType.Name;
            existingSeatType.Description = seatType.Description;
            existingSeatType.PriceMultiplier = seatType.PriceMultiplier;
            existingSeatType.IsActive = seatType.IsActive;

            await _seatTypeRepository.UpdateAsync(existingSeatType);
            return existingSeatType;
        }

        public async Task DeleteAsync(int id)
        {
            await _seatTypeRepository.DeleteAsync(id, "System", "Manual deletion");
        }
    }
} 