using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using SD_Sinema.Web.Services;

namespace SD_Sinema.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiService _apiService;

        public HomeController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Vizyondaki filmleri al
                var movieDtos = await _apiService.GetAsync<List<MovieDto>>("api/movies/active");
                var movies = movieDtos?.Select(m => new MovieViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description ?? string.Empty,
                    Duration = m.Duration,
                    ReleaseYear = m.ReleaseDate.Year,
                    GenreId = m.GenreId,
                    GenreName = m.GenreName ?? string.Empty,
                    CreatedAt = m.CreatedDate
                }).ToList() ?? new List<MovieViewModel>();

                // Yaklaşan seansları al
                var sessionDtos = await _apiService.GetAsync<List<SessionDto>>("api/sessions/active");
                var sessions = sessionDtos?.Select(s => new SessionViewModel
                {
                    Id = s.Id,
                    MovieId = s.MovieId,
                    SalonId = s.SalonId,
                    SessionDate = s.SessionDate,
                    StartTime = s.SessionDate.Add(s.StartTime),
                    EndTime = s.SessionDate.Add(s.EndTime),
                    IsActive = s.IsActive,
                    MovieTitle = s.MovieTitle,
                    SalonName = s.SalonName,
                    Price = s.Price
                }).ToList() ?? new List<SessionViewModel>();

                // İstatistikleri al
                var stats = await GetStatisticsAsync();

                var viewModel = new HomeViewModel
                {
                    ActiveMovies = movies,
                    UpcomingSessions = sessions.Take(6).ToList(), // İlk 6 seans
                    Statistics = stats
                };

                return View(viewModel);
            }
            catch
            {
                // API bağlantı hatası durumunda boş veriler göster
                return View(new HomeViewModel
                {
                    ActiveMovies = new List<MovieViewModel>(),
                    UpcomingSessions = new List<SessionViewModel>(),
                    Statistics = new StatisticsViewModel()
                });
            }
        }

        private async Task<StatisticsViewModel> GetStatisticsAsync()
        {
            var stats = new StatisticsViewModel();
            
            try
            {
                // Toplam film sayısı
                var movies = await _apiService.GetAsync<List<MovieDto>>("api/movies");
                stats.TotalMovies = movies?.Count ?? 0;

                // Toplam salon sayısı
                var salons = await _apiService.GetAsync<List<SalonDto>>("api/salons");
                stats.TotalSalons = salons?.Count ?? 0;

                // Toplam kullanıcı sayısı
                var users = await _apiService.GetAsync<List<UserDto>>("api/users");
                stats.TotalUsers = users?.Count ?? 0;

                // Toplam seans sayısı
                var sessions = await _apiService.GetAsync<List<SessionDto>>("api/sessions");
                stats.TotalSessions = sessions?.Count ?? 0;
            }
            catch
            {
                // Hata durumunda varsayılan değerler
            }

            return stats;
        }

        public async Task<IActionResult> About()
        {
            try
            {
                // İstatistikleri al
                var stats = await GetStatisticsAsync();
                ViewBag.Statistics = stats;
            }
            catch
            {
                ViewBag.Statistics = new StatisticsViewModel();
            }

            return View();
        }

        public async Task<IActionResult> Contact()
        {
            try
            {
                // Salon bilgilerini al
                var salons = await _apiService.GetAsync<List<SalonDto>>("api/salons");
                ViewBag.Salons = salons?.Take(3).ToList() ?? new List<SalonDto>();
            }
            catch
            {
                ViewBag.Salons = new List<SalonDto>();
            }

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
} 