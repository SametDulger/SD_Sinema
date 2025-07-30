using Microsoft.EntityFrameworkCore;
using SD_Sinema.Business.Services;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Context;
using SD_Sinema.Data.UnitOfWork;
using SD_Sinema.API.Middleware;

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

// Test ortamında InMemory database kullan, production'da SQL Server kullan
if (builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<SinemaDbContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.AddDbContext<SinemaDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ISalonService, SalonService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ITicketTypeService, TicketTypeService>();
builder.Services.AddScoped<ISeatService, SeatService>();

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