using Microsoft.EntityFrameworkCore;
using SD_Sinema.Core.Entities;

namespace SD_Sinema.Data.Context
{
    public class SinemaDbContext : DbContext
    {
        public SinemaDbContext(DbContextOptions<SinemaDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Salon> Salons { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatType> SeatTypes { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<SeatType>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.PriceMultiplier).HasColumnType("decimal(5,2)");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasOne(e => e.Genre)
                    .WithMany(g => g.Movies)
                    .HasForeignKey(e => e.GenreId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasOne(e => e.Salon)
                    .WithMany(s => s.Seats)
                    .HasForeignKey(e => e.SalonId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.SeatType)
                    .WithMany(st => st.Seats)
                    .HasForeignKey(e => e.SeatTypeId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.Sessions)
                    .HasForeignKey(e => e.MovieId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Salon)
                    .WithMany(s => s.Sessions)
                    .HasForeignKey(e => e.SalonId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Reservations)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Session)
                    .WithMany(s => s.Reservations)
                    .HasForeignKey(e => e.SessionId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Seat)
                    .WithMany(s => s.Reservations)
                    .HasForeignKey(e => e.SeatId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Tickets)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Session)
                    .WithMany(s => s.Tickets)
                    .HasForeignKey(e => e.SessionId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Seat)
                    .WithMany(s => s.Tickets)
                    .HasForeignKey(e => e.SeatId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.TicketType)
                    .WithMany(t => t.Tickets)
                    .HasForeignKey(e => e.TicketTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TicketType>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(5,2)");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });
        }
    }
} 