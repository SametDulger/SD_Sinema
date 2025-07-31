using Microsoft.EntityFrameworkCore;
using SD_Sinema.Business.Services;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Context;
using SD_Sinema.Data.UnitOfWork;
using SD_Sinema.Data.Repositories;
using SD_Sinema.Data.Seed;
using SD_Sinema.API.Middleware;
using SD_Sinema.Business.Mapping;
using FluentValidation;
using SD_Sinema.Business.Validation;

namespace SD_Sinema.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            builder.Services.AddValidatorsFromAssemblyContaining<CreateMovieDtoValidator>();

            // Sadece test ortamında InMemory, diğerlerinde SQL Server
            if (builder.Environment.EnvironmentName == "Test")
            {
                builder.Services.AddDbContext<SinemaDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            }
            else
            {
                builder.Services.AddDbContext<SinemaDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            }

            // Register generic repositories
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<ISalonService, SalonService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<ITicketTypeService, TicketTypeService>();
            builder.Services.AddScoped<ISeatService, SeatService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<ISeatTypeService, SeatTypeService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecific", policy =>
                {
                    policy.WithOrigins("http://localhost:5001", "https://localhost:7002")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Seed data'yı çalıştır
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SinemaDbContext>();
                context.Database.EnsureCreated();
                SeedData.SeedAsync(context).Wait();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecific");
            app.UseMiddleware<ValidationMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
} 