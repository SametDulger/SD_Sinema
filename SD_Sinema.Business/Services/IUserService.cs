using SD_Sinema.Business.DTOs;

namespace SD_Sinema.Business.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto updateUserDto);
        Task DeleteAsync(int id, string deletedBy, string reason);
        Task<UserDto?> LoginAsync(LoginDto loginDto);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<UserDto> MakeVipAsync(int id);
        Task<UserDto> CancelVipAsync(int id);
    }
} 