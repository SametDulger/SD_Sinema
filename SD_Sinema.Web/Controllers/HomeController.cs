using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using System.Text;

namespace SD_Sinema.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HomeController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/movies/active");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var movieDtos = JsonConvert.DeserializeObject<List<MovieDto>>(content);
                    
                    var movieViewModels = movieDtos?.Select(m => new MovieViewModel
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Description = m.Description ?? string.Empty,
                        Duration = m.Duration,
                        ReleaseYear = m.ReleaseDate.Year,
                        Genre = m.Genre ?? string.Empty,
                        CreatedAt = m.CreatedDate
                    }).ToList() ?? new List<MovieViewModel>();
                    
                    return View(movieViewModels);
                }
            }
            catch
            {
                // API bağlantı hatası durumunda boş liste göster
            }

            return View(Enumerable.Empty<MovieViewModel>());
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
} 