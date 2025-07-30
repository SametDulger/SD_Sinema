using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using System.Text;

namespace SD_Sinema.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly HttpClient _httpClient;

        public MoviesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/movies");
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
                // API bağlantı hatası
            }

            return View(Enumerable.Empty<MovieViewModel>());
        }

        public IActionResult Create()
        {
            return View(new MovieViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createMovieDto = new CreateMovieDto
                    {
                        Title = movieViewModel.Title,
                        Description = movieViewModel.Description,
                        Duration = movieViewModel.Duration,
                        Genre = movieViewModel.Genre,
                        Director = "",
                        Cast = "",
                        PosterUrl = "",
                        TrailerUrl = "",
                        ReleaseDate = new DateTime(movieViewModel.ReleaseYear, 1, 1),
                        EndDate = null
                    };

                    var json = JsonConvert.SerializeObject(createMovieDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/movies", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Film oluşturulurken hata oluştu.");
                }
            }

            return View(movieViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/movies/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var movieDto = JsonConvert.DeserializeObject<MovieDto>(content);
                    
                    if (movieDto == null)
                        return NotFound();
                    
                    var movieViewModel = new MovieViewModel
                    {
                        Id = movieDto.Id,
                        Title = movieDto.Title,
                        Description = movieDto.Description ?? string.Empty,
                        Duration = movieDto.Duration,
                        ReleaseYear = movieDto.ReleaseDate.Year,
                        Genre = movieDto.Genre ?? string.Empty,
                        CreatedAt = movieDto.CreatedDate
                    };
                    
                    return View(movieViewModel);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateMovieDto = new UpdateMovieDto
                    {
                        Title = movieViewModel.Title,
                        Description = movieViewModel.Description,
                        Duration = movieViewModel.Duration,
                        Genre = movieViewModel.Genre,
                        Director = "",
                        Cast = "",
                        PosterUrl = "",
                        TrailerUrl = "",
                        ReleaseDate = new DateTime(movieViewModel.ReleaseYear, 1, 1),
                        EndDate = null,
                        IsActive = true
                    };

                    var json = JsonConvert.SerializeObject(updateMovieDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"api/movies/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Film güncellenirken hata oluştu.");
                }
            }

            return View(movieViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/movies/{id}?deletedBy=Admin&reason=Silme");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/movies/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var movieDto = JsonConvert.DeserializeObject<MovieDto>(content);
                    
                    if (movieDto == null)
                        return NotFound();
                    
                    var movieViewModel = new MovieViewModel
                    {
                        Id = movieDto.Id,
                        Title = movieDto.Title,
                        Description = movieDto.Description ?? string.Empty,
                        Duration = movieDto.Duration,
                        ReleaseYear = movieDto.ReleaseDate.Year,
                        Genre = movieDto.Genre ?? string.Empty,
                        CreatedAt = movieDto.CreatedDate
                    };
                    
                    return View(movieViewModel);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return NotFound();
        }
    }
} 