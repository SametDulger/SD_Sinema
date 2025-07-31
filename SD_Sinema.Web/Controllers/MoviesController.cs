using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using SD_Sinema.Web.Services;

namespace SD_Sinema.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IApiService _apiService;

        public MoviesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var movieDtos = await _apiService.GetAsync<List<MovieDto>>("api/movies");
                
                var movieViewModels = movieDtos?.Select(m => new MovieViewModel
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
                
                return View(movieViewModels);
            }
            catch
            {
                // API bağlantı hatası
                return View(new List<MovieViewModel>());
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                // Film türleri için API'den veri al
                var genres = await _apiService.GetAsync<List<string>>("api/movies/genres");
                ViewBag.Genres = genres?.Select(g => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = g, Text = g }).ToList() 
                    ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Genres = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

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
                        GenreId = movieViewModel.GenreId,
                        Director = "",
                        Cast = "",
                        PosterUrl = "",
                        TrailerUrl = "",
                        ReleaseDate = new DateTime(movieViewModel.ReleaseYear, 1, 1),
                        EndDate = null
                    };

                    var result = await _apiService.PostAsync<MovieDto>("api/movies", createMovieDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Film oluşturulurken hata oluştu.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Film oluşturulurken hata oluştu: {ex.Message}");
                }
            }

            // Hata durumunda dropdown'ları tekrar doldur
            try
            {
                var genres = await _apiService.GetAsync<List<string>>("api/movies/genres");
                ViewBag.Genres = genres?.Select(g => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = g, Text = g }).ToList() 
                    ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Genres = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(movieViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var movieDto = await _apiService.GetAsync<MovieDto>($"api/movies/{id}");
                
                if (movieDto == null)
                    return NotFound();
                
                var movieViewModel = new MovieViewModel
                {
                    Id = movieDto.Id,
                    Title = movieDto.Title,
                    Description = movieDto.Description ?? string.Empty,
                    Duration = movieDto.Duration,
                    ReleaseYear = movieDto.ReleaseDate.Year,
                    GenreId = movieDto.GenreId,
                    GenreName = movieDto.GenreName ?? string.Empty,
                    CreatedAt = movieDto.CreatedDate
                };

                // Film türleri için API'den veri al
                var genres = await _apiService.GetAsync<List<string>>("api/movies/genres");
                ViewBag.Genres = genres?.Select(g => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = g, Text = g }).ToList() 
                    ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                
                return View(movieViewModel);
            }
            catch
            {
                return NotFound();
            }
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
                        GenreId = movieViewModel.GenreId,
                        Director = "",
                        Cast = "",
                        PosterUrl = "",
                        TrailerUrl = "",
                        ReleaseDate = new DateTime(movieViewModel.ReleaseYear, 1, 1),
                        EndDate = null
                    };

                    var result = await _apiService.PutAsync<MovieDto>($"api/movies/{id}", updateMovieDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Film güncellenirken hata oluştu.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Film güncellenirken hata oluştu: {ex.Message}");
                }
            }

            // Hata durumunda dropdown'ları tekrar doldur
            try
            {
                var genres = await _apiService.GetAsync<List<string>>("api/movies/genres");
                ViewBag.Genres = genres?.Select(g => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = g, Text = g }).ToList() 
                    ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Genres = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(movieViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var movieDto = await _apiService.GetAsync<MovieDto>($"api/movies/{id}");
                
                if (movieDto == null)
                    return NotFound();
                
                var movieViewModel = new MovieViewModel
                {
                    Id = movieDto.Id,
                    Title = movieDto.Title,
                    Description = movieDto.Description ?? string.Empty,
                    Duration = movieDto.Duration,
                    ReleaseYear = movieDto.ReleaseDate.Year,
                    GenreId = movieDto.GenreId,
                    GenreName = movieDto.GenreName ?? string.Empty,
                    CreatedAt = movieDto.CreatedDate
                };
                
                return View(movieViewModel);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _apiService.DeleteAsync($"api/movies/{id}");
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Film silinirken hata oluştu.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Film silinirken hata oluştu: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var movieDto = await _apiService.GetAsync<MovieDto>($"api/movies/{id}");
                
                if (movieDto == null)
                    return NotFound();
                
                var movieViewModel = new MovieViewModel
                {
                    Id = movieDto.Id,
                    Title = movieDto.Title,
                    Description = movieDto.Description ?? string.Empty,
                    Duration = movieDto.Duration,
                    ReleaseYear = movieDto.ReleaseDate.Year,
                    GenreId = movieDto.GenreId,
                    GenreName = movieDto.GenreName ?? string.Empty,
                    CreatedAt = movieDto.CreatedDate
                };

                // Film seanslarını da al
                var sessionDtos = await _apiService.GetAsync<List<SessionDto>>($"api/sessions/movie/{id}");
                ViewBag.Sessions = sessionDtos?.Take(5).ToList() ?? new List<SessionDto>();
                
                return View(movieViewModel);
            }
            catch
            {
                return NotFound();
            }
        }
    }
} 