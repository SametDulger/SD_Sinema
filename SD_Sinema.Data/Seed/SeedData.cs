using Microsoft.EntityFrameworkCore;
using SD_Sinema.Core.Entities;
using SD_Sinema.Data.Context;

namespace SD_Sinema.Data.Seed
{
    public static class SeedData
    {
        public static async Task SeedAsync(SinemaDbContext context)
        {
            // Genre verilerini ekle
            if (!await context.Genres.AnyAsync())
            {
                var genres = new List<Genre>
                {
                    new Genre { Name = "Aksiyon", Description = "Aksiyon filmleri", IsActive = true },
                    new Genre { Name = "Komedi", Description = "Komedi filmleri", IsActive = true },
                    new Genre { Name = "Drama", Description = "Drama filmleri", IsActive = true },
                    new Genre { Name = "Bilim Kurgu", Description = "Bilim kurgu filmleri", IsActive = true },
                    new Genre { Name = "Korku", Description = "Korku filmleri", IsActive = true },
                    new Genre { Name = "Romantik", Description = "Romantik filmleri", IsActive = true },
                    new Genre { Name = "Macera", Description = "Macera filmleri", IsActive = true },
                    new Genre { Name = "Animasyon", Description = "Animasyon filmleri", IsActive = true },
                    new Genre { Name = "Belgesel", Description = "Belgesel filmleri", IsActive = true },
                    new Genre { Name = "Gerilim", Description = "Gerilim filmleri", IsActive = true },
                    new Genre { Name = "Fantastik", Description = "Fantastik filmleri", IsActive = true },
                    new Genre { Name = "Suç", Description = "Suç filmleri", IsActive = true },
                    new Genre { Name = "Western", Description = "Western filmleri", IsActive = true },
                    new Genre { Name = "Müzikal", Description = "Müzikal filmleri", IsActive = true },
                    new Genre { Name = "Tarih", Description = "Tarih filmleri", IsActive = true }
                };

                await context.Genres.AddRangeAsync(genres);
                await context.SaveChangesAsync();
            }

            // SeatType verilerini ekle
            if (!await context.SeatTypes.AnyAsync())
            {
                var seatTypes = new List<SeatType>
                {
                    new SeatType { Name = "Standard", Description = "Standart koltuk", PriceMultiplier = 1.0m, IsActive = true },
                    new SeatType { Name = "VIP", Description = "VIP koltuk", PriceMultiplier = 1.5m, IsActive = true },
                    new SeatType { Name = "Premium", Description = "Premium koltuk", PriceMultiplier = 2.0m, IsActive = true },
                    new SeatType { Name = "Wheelchair", Description = "Tekerlekli sandalye koltuk", PriceMultiplier = 0.8m, IsActive = true },
                    new SeatType { Name = "Couple", Description = "Çift koltuk", PriceMultiplier = 1.8m, IsActive = true }
                };

                await context.SeatTypes.AddRangeAsync(seatTypes);
                await context.SaveChangesAsync();
            }

            // TicketType verilerini ekle
            if (!await context.TicketTypes.AnyAsync())
            {
                var ticketTypes = new List<TicketType>
                {
                    new TicketType { Name = "Tam Bilet", Description = "Yetişkin tam bilet", Price = 50.00m, DiscountPercentage = 0, IsActive = true },
                    new TicketType { Name = "Öğrenci Bileti", Description = "Öğrenci bileti", Price = 35.00m, DiscountPercentage = 30, IsActive = true },
                    new TicketType { Name = "Çocuk Bileti", Description = "12 yaş altı çocuk bileti", Price = 25.00m, DiscountPercentage = 50, IsActive = true },
                    new TicketType { Name = "Yaşlı Bileti", Description = "65 yaş üstü bileti", Price = 30.00m, DiscountPercentage = 40, IsActive = true },
                    new TicketType { Name = "Engelli Bileti", Description = "Engelli bileti", Price = 20.00m, DiscountPercentage = 60, IsActive = true }
                };

                await context.TicketTypes.AddRangeAsync(ticketTypes);
                await context.SaveChangesAsync();
            }

            // Salon verilerini ekle
            if (!await context.Salons.AnyAsync())
            {
                var salons = new List<Salon>
                {
                    new Salon { Name = "Salon 1", Capacity = 120, Description = "Ana salon", IsActive = true },
                    new Salon { Name = "Salon 2", Capacity = 80, Description = "Küçük salon", IsActive = true },
                    new Salon { Name = "Salon 3", Capacity = 60, Description = "VIP salon", IsActive = true },
                    new Salon { Name = "Salon 4", Capacity = 100, Description = "3D salon", IsActive = true },
                    new Salon { Name = "Salon 5", Capacity = 150, Description = "Büyük salon", IsActive = true }
                };

                await context.Salons.AddRangeAsync(salons);
                await context.SaveChangesAsync();
            }

            // Film verilerini ekle
            if (!await context.Movies.AnyAsync())
            {
                var genres = await context.Genres.ToListAsync();
                var movies = new List<Movie>
                {
                    new Movie 
                    { 
                        Title = "Inception", 
                        Description = "Rüya içinde rüya konseptini işleyen bilim kurgu filmi", 
                        Duration = 148, 
                        Director = "Christopher Nolan",
                        Cast = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page",
                        GenreId = genres.FirstOrDefault(g => g.Name == "Bilim Kurgu")?.Id,
                        AgeRating = "13+",
                        ReleaseDate = new DateTime(2010, 7, 16),
                        IsActive = true
                    },
                    new Movie 
                    { 
                        Title = "The Dark Knight", 
                        Description = "Batman'in Joker ile mücadelesini anlatan süper kahraman filmi", 
                        Duration = 152, 
                        Director = "Christopher Nolan",
                        Cast = "Christian Bale, Heath Ledger, Aaron Eckhart",
                        GenreId = genres.FirstOrDefault(g => g.Name == "Aksiyon")?.Id,
                        AgeRating = "13+",
                        ReleaseDate = new DateTime(2008, 7, 18),
                        IsActive = true
                    },
                    new Movie 
                    { 
                        Title = "Forrest Gump", 
                        Description = "Basit bir adamın hayatını anlatan drama filmi", 
                        Duration = 142, 
                        Director = "Robert Zemeckis",
                        Cast = "Tom Hanks, Robin Wright, Gary Sinise",
                        GenreId = genres.FirstOrDefault(g => g.Name == "Drama")?.Id,
                        AgeRating = "13+",
                        ReleaseDate = new DateTime(1994, 7, 6),
                        IsActive = true
                    },
                    new Movie 
                    { 
                        Title = "The Lion King", 
                        Description = "Simba'nın hikayesini anlatan animasyon filmi", 
                        Duration = 88, 
                        Director = "Roger Allers, Rob Minkoff",
                        Cast = "Matthew Broderick, James Earl Jones, Jeremy Irons",
                        GenreId = genres.FirstOrDefault(g => g.Name == "Animasyon")?.Id,
                        AgeRating = "G",
                        ReleaseDate = new DateTime(1994, 6, 24),
                        IsActive = true
                    },
                    new Movie 
                    { 
                        Title = "Titanic", 
                        Description = "RMS Titanic gemisinde geçen aşk hikayesi", 
                        Duration = 194, 
                        Director = "James Cameron",
                        Cast = "Leonardo DiCaprio, Kate Winslet, Billy Zane",
                        GenreId = genres.FirstOrDefault(g => g.Name == "Romantik")?.Id,
                        AgeRating = "13+",
                        ReleaseDate = new DateTime(1997, 12, 19),
                        IsActive = true
                    }
                };

                await context.Movies.AddRangeAsync(movies);
                await context.SaveChangesAsync();
            }

            // Koltuk verilerini ekle
            if (!await context.Seats.AnyAsync())
            {
                var salons = await context.Salons.ToListAsync();
                var seatTypes = await context.SeatTypes.ToListAsync();
                var seats = new List<Seat>();

                foreach (var salon in salons)
                {
                    var standardSeatType = seatTypes.FirstOrDefault(st => st.Name == "Standard");
                    var vipSeatType = seatTypes.FirstOrDefault(st => st.Name == "VIP");
                    var wheelchairSeatType = seatTypes.FirstOrDefault(st => st.Name == "Wheelchair");

                    // Her salon için koltuklar oluştur
                    for (int row = 1; row <= 10; row++)
                    {
                        for (int seatNum = 1; seatNum <= 12; seatNum++)
                        {
                            var seatType = standardSeatType;
                            
                            // VIP koltuklar (son 2 sıra)
                            if (row >= 9)
                            {
                                seatType = vipSeatType;
                            }
                            
                            // Tekerlekli sandalye koltukları (ilk sıra, 1-2 numara)
                            if (row == 1 && (seatNum == 1 || seatNum == 2))
                            {
                                seatType = wheelchairSeatType;
                            }

                            seats.Add(new Seat
                            {
                                SalonId = salon.Id,
                                RowNumber = row.ToString(),
                                SeatNumber = seatNum,
                                SeatTypeId = seatType?.Id,
                                IsActive = true,
                                IsAvailable = true
                            });
                        }
                    }
                }

                await context.Seats.AddRangeAsync(seats);
                await context.SaveChangesAsync();
            }

            // Kullanıcı verilerini ekle
            if (!await context.Users.AnyAsync())
            {
                // Şifre hash'leme fonksiyonu
                string HashPassword(string password)
                {
                    using var sha256 = System.Security.Cryptography.SHA256.Create();
                    var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    return Convert.ToBase64String(hashedBytes);
                }

                var users = new List<User>
                {
                    new User 
                    { 
                        FirstName = "Admin", 
                        LastName = "User", 
                        Email = "admin@sinema.com", 
                        Password = HashPassword("admin123"), 
                        PhoneNumber = "555-0001",
                        BirthDate = new DateTime(1990, 1, 1),
                        Gender = "Erkek",
                        Profession = "Yönetici",
                        IsActive = true
                    },
                    new User 
                    { 
                        FirstName = "Ahmet", 
                        LastName = "Yılmaz", 
                        Email = "ahmet@email.com", 
                        Password = HashPassword("123456"), 
                        PhoneNumber = "555-0002",
                        BirthDate = new DateTime(1985, 5, 15),
                        Gender = "Erkek",
                        Profession = "Mühendis",
                        IsActive = true
                    },
                    new User 
                    { 
                        FirstName = "Ayşe", 
                        LastName = "Demir", 
                        Email = "ayse@email.com", 
                        Password = HashPassword("123456"), 
                        PhoneNumber = "555-0003",
                        BirthDate = new DateTime(1992, 8, 22),
                        Gender = "Kadın",
                        Profession = "Öğretmen",
                        IsActive = true
                    }
                };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }

            // Seans verilerini ekle
            if (!await context.Sessions.AnyAsync())
            {
                var movies = await context.Movies.ToListAsync();
                var salons = await context.Salons.ToListAsync();
                var sessions = new List<Session>();

                var startDate = DateTime.Now.Date.AddDays(1); // Yarın başla

                foreach (var movie in movies.Take(3)) // İlk 3 film için seans oluştur
                {
                    foreach (var salon in salons.Take(2)) // İlk 2 salon için
                    {
                        // Her film için 3 seans oluştur (10:00, 14:00, 18:00)
                        var times = new[] { 10, 14, 18 };
                        foreach (var hour in times)
                        {
                            sessions.Add(new Session
                            {
                                MovieId = movie.Id,
                                SalonId = salon.Id,
                                SessionDate = startDate,
                                StartTime = TimeSpan.FromHours(hour),
                                EndTime = TimeSpan.FromHours(hour).Add(TimeSpan.FromMinutes(movie.Duration)),
                                Price = 50.00m,
                                IsActive = true
                            });
                        }
                    }
                }

                await context.Sessions.AddRangeAsync(sessions);
                await context.SaveChangesAsync();
            }

            // Rezervasyon verilerini ekle
            if (!await context.Reservations.AnyAsync())
            {
                var users = await context.Users.ToListAsync();
                var sessions = await context.Sessions.ToListAsync();
                var seats = await context.Seats.ToListAsync();
                var reservations = new List<Reservation>();

                // Her kullanıcı için 1-2 rezervasyon oluştur
                foreach (var user in users)
                {
                    var userSessions = sessions.Take(2).ToList(); // İlk 2 seans
                    foreach (var session in userSessions)
                    {
                        var sessionSeats = seats.Where(s => s.SalonId == session.SalonId).Take(2).ToList();
                        foreach (var seat in sessionSeats)
                        {
                            reservations.Add(new Reservation
                            {
                                UserId = user.Id,
                                SessionId = session.Id,
                                SeatId = seat.Id,
                                ReservationDate = DateTime.Now,
                                ExpiryDate = DateTime.Now.AddHours(2), // 2 saat geçerli
                                Status = "Confirmed"
                            });
                        }
                    }
                }

                await context.Reservations.AddRangeAsync(reservations);
                await context.SaveChangesAsync();
            }

            // Bilet verilerini ekle
            if (!await context.Tickets.AnyAsync())
            {
                var reservations = await context.Reservations.ToListAsync();
                var ticketTypes = await context.TicketTypes.ToListAsync();
                var tickets = new List<Ticket>();

                foreach (var reservation in reservations.Take(5)) // İlk 5 rezervasyon için bilet oluştur
                {
                    var ticketType = ticketTypes.FirstOrDefault(tt => tt.Name == "Tam Bilet");
                    if (ticketType != null)
                    {
                        tickets.Add(new Ticket
                        {
                            UserId = reservation.UserId,
                            SessionId = reservation.SessionId,
                            SeatId = reservation.SeatId,
                            TicketTypeId = ticketType.Id,
                            Price = ticketType.Price,
                            PurchaseDate = DateTime.Now,
                            Status = "Active"
                        });
                    }
                }

                await context.Tickets.AddRangeAsync(tickets);
                await context.SaveChangesAsync();
            }
        }
    }
} 