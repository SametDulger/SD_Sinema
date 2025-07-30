using System.Security.Cryptography;
using System.Text;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;

namespace SD_Sinema.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
        {
            if (createUserDto == null)
                throw new ArgumentNullException(nameof(createUserDto));

            if (await EmailExistsAsync(createUserDto.Email))
                throw new InvalidOperationException("Bu email adresi zaten kullanılıyor.");

            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                Password = HashPassword(createUserDto.Password),
                PhoneNumber = createUserDto.Phone,
                BirthDate = createUserDto.BirthDate,
                Gender = createUserDto.Gender,
                Profession = createUserDto.Profession
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
                throw new ArgumentNullException(nameof(updateUserDto));

            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                throw new InvalidOperationException("Kullanıcı bulunamadı.");

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.PhoneNumber = updateUserDto.Phone;
            user.BirthDate = updateUserDto.BirthDate;
            user.Gender = updateUserDto.Gender;
            user.Profession = updateUserDto.Profession;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task DeleteAsync(int id, string deletedBy, string reason)
        {
            await _unitOfWork.Users.DeleteAsync(id, deletedBy, reason);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<UserDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.GetAllAsync();
            var foundUser = user.FirstOrDefault(u => u.Email == loginDto.Email && 
                                                   VerifyPassword(loginDto.Password, u.Password) && 
                                                   u.IsActive);

            return foundUser != null ? MapToDto(foundUser) : null;
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == email);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Users.ExistsAsync(id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Any(u => u.Email == email);
        }

        public async Task<UserDto> MakeVipAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                throw new InvalidOperationException("Kullanıcı bulunamadı.");

            user.IsVipMember = true;
            user.VipStartDate = DateTime.Now;
            user.VipEndDate = DateTime.Now.AddYears(1);
            user.MonthlyFreeMovieCount = 2;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<UserDto> CancelVipAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                throw new InvalidOperationException("Kullanıcı bulunamadı.");

            user.IsVipMember = false;
            user.VipEndDate = DateTime.Now;
            user.MonthlyFreeMovieCount = 0;
            user.UsedFreeMovieCount = 0;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(user);
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Profession = user.Profession,
                IsActive = user.IsActive,
                IsEmailConfirmed = user.IsEmailConfirmed,
                IsVipMember = user.IsVipMember,
                VipStartDate = user.VipStartDate,
                VipEndDate = user.VipEndDate,
                MonthlyFreeMovieCount = user.MonthlyFreeMovieCount,
                UsedFreeMovieCount = user.UsedFreeMovieCount,
                Role = user.Role,
                CreatedDate = user.CreatedDate
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
} 