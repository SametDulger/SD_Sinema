using SD_Sinema.Core.Entities;

namespace SD_Sinema.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int id, string deletedBy, string reason);
        Task<bool> ExistsAsync(int id);
        Task<int> SaveChangesAsync();
    }

    public interface IMovieRepository : IRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetAllWithSessionsAsync();
        Task<Movie?> GetByIdWithSessionsAsync(int id);
        Task<IEnumerable<Movie>> GetActiveMoviesAsync();
    }

    public interface ISessionRepository : IRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllWithDetailsAsync();
        Task<Session?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Session>> GetSessionsByMovieAsync(int movieId);
        Task<IEnumerable<Session>> GetSessionsBySalonAsync(int salonId);
    }

    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetAllWithDetailsAsync();
        Task<Reservation?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Reservation>> GetReservationsByUserAsync(int userId);
        Task<IEnumerable<Reservation>> GetReservationsBySessionAsync(int sessionId);
    }
} 