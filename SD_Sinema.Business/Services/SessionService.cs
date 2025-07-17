using SD_Sinema.Business.DTOs;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;

namespace SD_Sinema.Business.Services
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SessionDto>> GetAllAsync()
        {
            var sessions = await _unitOfWork.Sessions.GetAllWithDetailsAsync();
            return sessions.Select(MapToDto);
        }

        public async Task<SessionDto?> GetByIdAsync(int id)
        {
            var session = await _unitOfWork.Sessions.GetByIdWithDetailsAsync(id);
            return session != null ? MapToDto(session) : null;
        }

        public async Task<SessionDto> CreateAsync(CreateSessionDto createSessionDto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(createSessionDto.MovieId);
            if (movie == null)
                throw new InvalidOperationException("Film bulunamadı.");

            var session = new Session
            {
                MovieId = createSessionDto.MovieId,
                SalonId = createSessionDto.SalonId,
                SessionDate = createSessionDto.SessionDate,
                StartTime = createSessionDto.StartTime,
                EndTime = createSessionDto.StartTime.Add(TimeSpan.FromMinutes(movie.Duration)),
                IsSpecialSession = createSessionDto.IsSpecialSession,
                SpecialSessionName = createSessionDto.SpecialSessionName,
                Price = createSessionDto.Price
            };

            await _unitOfWork.Sessions.AddAsync(session);
            await _unitOfWork.SaveChangesAsync();

            var sessionWithDetails = await _unitOfWork.Sessions.GetByIdWithDetailsAsync(session.Id);
            return MapToDto(sessionWithDetails!);
        }

        public async Task<SessionDto> UpdateAsync(int id, UpdateSessionDto updateSessionDto)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(id);
            if (session == null)
                throw new InvalidOperationException("Seans bulunamadı.");

            var movie = await _unitOfWork.Movies.GetByIdAsync(updateSessionDto.MovieId);
            if (movie == null)
                throw new InvalidOperationException("Film bulunamadı.");

            session.MovieId = updateSessionDto.MovieId;
            session.SalonId = updateSessionDto.SalonId;
            session.SessionDate = updateSessionDto.SessionDate;
            session.StartTime = updateSessionDto.StartTime;
            session.EndTime = updateSessionDto.StartTime.Add(TimeSpan.FromMinutes(movie.Duration));
            session.IsActive = updateSessionDto.IsActive;
            session.IsSpecialSession = updateSessionDto.IsSpecialSession;
            session.SpecialSessionName = updateSessionDto.SpecialSessionName;
            session.Price = updateSessionDto.Price;

            await _unitOfWork.Sessions.UpdateAsync(session);
            await _unitOfWork.SaveChangesAsync();

            var sessionWithDetails = await _unitOfWork.Sessions.GetByIdWithDetailsAsync(session.Id);
            return MapToDto(sessionWithDetails!);
        }

        public async Task DeleteAsync(int id, string deletedBy, string reason)
        {
            await _unitOfWork.Sessions.DeleteAsync(id, deletedBy, reason);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Sessions.ExistsAsync(id);
        }

        public async Task<IEnumerable<SessionDto>> GetSessionsByMovieAsync(int movieId)
        {
            var sessions = await _unitOfWork.Sessions.GetSessionsByMovieAsync(movieId);
            return sessions.Where(s => s.IsActive).Select(MapToDto);
        }

        public async Task<IEnumerable<SessionDto>> GetSessionsBySalonAsync(int salonId)
        {
            var sessions = await _unitOfWork.Sessions.GetSessionsBySalonAsync(salonId);
            return sessions.Where(s => s.IsActive).Select(MapToDto);
        }

        public async Task<IEnumerable<SessionDto>> GetActiveSessionsAsync()
        {
            var sessions = await _unitOfWork.Sessions.GetAllWithDetailsAsync();
            return sessions.Where(s => s.IsActive && s.SessionDate >= DateTime.Today).Select(MapToDto);
        }

        private static SessionDto MapToDto(Session session)
        {
            return new SessionDto
            {
                Id = session.Id,
                MovieId = session.MovieId,
                SalonId = session.SalonId,
                SessionDate = session.SessionDate,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                IsActive = session.IsActive,
                IsSpecialSession = session.IsSpecialSession,
                SpecialSessionName = session.SpecialSessionName,
                MovieTitle = session.Movie?.Title ?? "",
                SalonName = session.Salon?.Name ?? "",
                CreatedDate = session.CreatedDate,
                Price = session.Price
            };
        }
    }
} 