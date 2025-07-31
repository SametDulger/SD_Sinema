using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using SD_Sinema.Web.Services;

namespace SD_Sinema.Web.Controllers
{
    public class SessionsController : Controller
    {
        private readonly IApiService _apiService;

        public SessionsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var sessionDtos = await _apiService.GetAsync<List<SessionDto>>("api/sessions");
                
                var sessionViewModels = sessionDtos?.Select(s => new SessionViewModel
                {
                    Id = s.Id,
                    MovieId = s.MovieId,
                    SalonId = s.SalonId,
                    SessionDate = s.SessionDate,
                    StartTime = s.SessionDate.Add(s.StartTime),
                    EndTime = s.SessionDate.Add(s.EndTime),
                    IsActive = s.IsActive,
                    IsSpecialSession = s.IsSpecialSession,
                    SpecialSessionName = s.SpecialSessionName,
                    Price = s.Price,
                    CreatedAt = s.CreatedDate,
                    MovieTitle = s.MovieTitle,
                    SalonName = s.SalonName
                }).ToArray() ?? new SessionViewModel[0];
                
                // Ensure we return a List, not an array
                var sessionList = new List<SessionViewModel>(sessionViewModels);
                
                return View(sessionList);
            }
            catch
            {
                // API bağlantı hatası
                return View(new List<SessionViewModel>());
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                // Film listesini yükle
                var movieDtos = await _apiService.GetAsync<List<MovieDto>>("api/movies");
                ViewBag.Movies = movieDtos?.Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Title
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                // Salon listesini yükle
                var salonDtos = await _apiService.GetAsync<List<SalonDto>>("api/salons");
                ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Movies = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Salons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(new SessionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SessionViewModel sessionViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createSessionDto = new CreateSessionDto
                    {
                        MovieId = sessionViewModel.MovieId,
                        SalonId = sessionViewModel.SalonId,
                        SessionDate = sessionViewModel.SessionDate,
                        StartTime = sessionViewModel.StartTime.TimeOfDay,
                        EndTime = sessionViewModel.EndTime.TimeOfDay,
                        IsActive = sessionViewModel.IsActive,
                        IsSpecialSession = sessionViewModel.IsSpecialSession,
                        SpecialSessionName = sessionViewModel.SpecialSessionName,
                        Price = sessionViewModel.Price
                    };

                    var result = await _apiService.PostAsync<SessionDto>("api/sessions", createSessionDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Seans oluşturulurken hata oluştu.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Seans oluşturulurken hata oluştu: {ex.Message}");
                }
            }

            // Hata durumunda dropdown'ları tekrar doldur
            try
            {
                // Film listesini yükle
                var movieDtos = await _apiService.GetAsync<List<MovieDto>>("api/movies");
                ViewBag.Movies = movieDtos?.Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Title
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                // Salon listesini yükle
                var salonDtos = await _apiService.GetAsync<List<SalonDto>>("api/salons");
                ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Movies = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Salons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(sessionViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var sessionDto = await _apiService.GetAsync<SessionDto>($"api/sessions/{id}");
                
                if (sessionDto == null)
                    return NotFound();
                
                var sessionViewModel = new SessionViewModel
                {
                    Id = sessionDto.Id,
                    MovieId = sessionDto.MovieId,
                    SalonId = sessionDto.SalonId,
                    SessionDate = sessionDto.SessionDate,
                    StartTime = sessionDto.SessionDate.Add(sessionDto.StartTime),
                    EndTime = sessionDto.SessionDate.Add(sessionDto.EndTime),
                    IsActive = sessionDto.IsActive,
                    IsSpecialSession = sessionDto.IsSpecialSession,
                    SpecialSessionName = sessionDto.SpecialSessionName,
                    Price = sessionDto.Price,
                    CreatedAt = sessionDto.CreatedDate,
                    MovieTitle = sessionDto.MovieTitle,
                    SalonName = sessionDto.SalonName
                };

                // Film listesini yükle
                var movieDtos = await _apiService.GetAsync<List<MovieDto>>("api/movies");
                ViewBag.Movies = movieDtos?.Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Title
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                // Salon listesini yükle
                var salonDtos = await _apiService.GetAsync<List<SalonDto>>("api/salons");
                ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                
                return View(sessionViewModel);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SessionViewModel sessionViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateSessionDto = new UpdateSessionDto
                    {
                        Id = id,
                        MovieId = sessionViewModel.MovieId,
                        SalonId = sessionViewModel.SalonId,
                        SessionDate = sessionViewModel.SessionDate,
                        StartTime = sessionViewModel.StartTime.TimeOfDay,
                        EndTime = sessionViewModel.EndTime.TimeOfDay,
                        IsActive = sessionViewModel.IsActive,
                        IsSpecialSession = sessionViewModel.IsSpecialSession,
                        SpecialSessionName = sessionViewModel.SpecialSessionName,
                        Price = sessionViewModel.Price
                    };

                    var result = await _apiService.PutAsync<SessionDto>($"api/sessions/{id}", updateSessionDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Seans güncellenirken hata oluştu.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Seans güncellenirken hata oluştu: {ex.Message}");
                }
            }

            // Hata durumunda dropdown'ları tekrar doldur
            try
            {
                // Film listesini yükle
                var movieDtos = await _apiService.GetAsync<List<MovieDto>>("api/movies");
                ViewBag.Movies = movieDtos?.Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Title
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                // Salon listesini yükle
                var salonDtos = await _apiService.GetAsync<List<SalonDto>>("api/salons");
                ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Movies = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Salons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(sessionViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var sessionDto = await _apiService.GetAsync<SessionDto>($"api/sessions/{id}");
                
                if (sessionDto == null)
                    return NotFound();
                
                var sessionViewModel = new SessionViewModel
                {
                    Id = sessionDto.Id,
                    MovieId = sessionDto.MovieId,
                    SalonId = sessionDto.SalonId,
                    SessionDate = sessionDto.SessionDate,
                    StartTime = sessionDto.SessionDate.Add(sessionDto.StartTime),
                    EndTime = sessionDto.SessionDate.Add(sessionDto.EndTime),
                    IsActive = sessionDto.IsActive,
                    IsSpecialSession = sessionDto.IsSpecialSession,
                    SpecialSessionName = sessionDto.SpecialSessionName,
                    Price = sessionDto.Price,
                    CreatedAt = sessionDto.CreatedDate,
                    MovieTitle = sessionDto.MovieTitle,
                    SalonName = sessionDto.SalonName
                };
                
                return View(sessionViewModel);
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
                var success = await _apiService.DeleteAsync($"api/sessions/{id}");
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Seans silinirken hata oluştu.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Seans silinirken hata oluştu: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var sessionDto = await _apiService.GetAsync<SessionDto>($"api/sessions/{id}");
                
                if (sessionDto == null)
                    return NotFound();
                
                var sessionViewModel = new SessionViewModel
                {
                    Id = sessionDto.Id,
                    MovieId = sessionDto.MovieId,
                    SalonId = sessionDto.SalonId,
                    SessionDate = sessionDto.SessionDate,
                    StartTime = sessionDto.SessionDate.Add(sessionDto.StartTime),
                    EndTime = sessionDto.SessionDate.Add(sessionDto.EndTime),
                    IsActive = sessionDto.IsActive,
                    IsSpecialSession = sessionDto.IsSpecialSession,
                    SpecialSessionName = sessionDto.SpecialSessionName,
                    Price = sessionDto.Price,
                    CreatedAt = sessionDto.CreatedDate,
                    MovieTitle = sessionDto.MovieTitle,
                    SalonName = sessionDto.SalonName
                };
                
                return View(sessionViewModel);
            }
            catch
            {
                return NotFound();
            }
        }
    }
} 