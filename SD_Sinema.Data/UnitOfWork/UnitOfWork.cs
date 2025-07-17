using Microsoft.EntityFrameworkCore.Storage;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Context;
using SD_Sinema.Data.Repositories;

namespace SD_Sinema.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SinemaDbContext _context;
        private IDbContextTransaction? _transaction;

        private IRepository<User>? _users;
        private IRepository<Salon>? _salons;
        private IRepository<Seat>? _seats;
        private IMovieRepository? _movies;
        private ISessionRepository? _sessions;
        private IRepository<TicketType>? _ticketTypes;
        private IReservationRepository? _reservations;
        private IRepository<Ticket>? _tickets;

        public UnitOfWork(SinemaDbContext context)
        {
            _context = context;
        }

        public IRepository<User> Users => _users ??= new Repository<User>(_context);
        public IRepository<Salon> Salons => _salons ??= new Repository<Salon>(_context);
        public IRepository<Seat> Seats => _seats ??= new Repository<Seat>(_context);
        public IMovieRepository Movies => _movies ??= new MovieRepository(_context);
        public ISessionRepository Sessions => _sessions ??= new SessionRepository(_context);
        public IRepository<TicketType> TicketTypes => _ticketTypes ??= new Repository<TicketType>(_context);
        public IReservationRepository Reservations => _reservations ??= new ReservationRepository(_context);
        public IRepository<Ticket> Tickets => _tickets ??= new Repository<Ticket>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
} 