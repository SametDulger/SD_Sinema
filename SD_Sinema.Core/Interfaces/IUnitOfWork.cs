using SD_Sinema.Core.Entities;

namespace SD_Sinema.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Salon> Salons { get; }
        IRepository<Seat> Seats { get; }
        IMovieRepository Movies { get; }
        ISessionRepository Sessions { get; }
        IRepository<TicketType> TicketTypes { get; }
        IReservationRepository Reservations { get; }
        IRepository<Ticket> Tickets { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
} 