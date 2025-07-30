using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using System.Text;

namespace SD_Sinema.Web.Controllers
{
    public class SessionsController : Controller
    {
        private readonly HttpClient _httpClient;

        public SessionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/sessions");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var sessionDtos = JsonConvert.DeserializeObject<List<SessionDto>>(content);
                    
                    var sessionViewModels = sessionDtos?.Select(s => new SessionViewModel
                    {
                        Id = s.Id,
                        MovieId = s.MovieId,
                        SalonId = s.SalonId,
                        StartTime = s.SessionDate.Date.Add(s.StartTime),
                        Price = s.Price,
                        IsActive = s.IsActive,
                        CreatedAt = s.CreatedDate,
                        MovieTitle = s.MovieTitle,
                        SalonName = s.SalonName
                    }).ToList() ?? new List<SessionViewModel>();
                    
                    return View(sessionViewModels);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return View(Enumerable.Empty<SessionViewModel>());
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                // Film listesini yükle
                var moviesResponse = await _httpClient.GetAsync("api/movies");
                if (moviesResponse.IsSuccessStatusCode)
                {
                    var moviesContent = await moviesResponse.Content.ReadAsStringAsync();
                    var movieDtos = JsonConvert.DeserializeObject<List<MovieDto>>(moviesContent);
                    ViewBag.Movies = movieDtos?.Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Title
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                // Salon listesini yükle
                var salonsResponse = await _httpClient.GetAsync("api/salons");
                if (salonsResponse.IsSuccessStatusCode)
                {
                    var salonsContent = await salonsResponse.Content.ReadAsStringAsync();
                    var salonDtos = JsonConvert.DeserializeObject<List<SalonDto>>(salonsContent);
                    ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
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
                        SessionDate = sessionViewModel.StartTime.Date,
                        StartTime = sessionViewModel.StartTime.TimeOfDay,
                        Price = sessionViewModel.Price
                    };

                    var json = JsonConvert.SerializeObject(createSessionDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/sessions", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Seans oluşturulurken hata oluştu.");
                }
            }

            // Hata durumunda film ve salon listelerini tekrar yükle
            try
            {
                var moviesResponse = await _httpClient.GetAsync("api/movies");
                if (moviesResponse.IsSuccessStatusCode)
                {
                    var moviesContent = await moviesResponse.Content.ReadAsStringAsync();
                    var movieDtos = JsonConvert.DeserializeObject<List<MovieDto>>(moviesContent);
                    ViewBag.Movies = movieDtos?.Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Title
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                else
                {
                    ViewBag.Movies = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                var salonsResponse = await _httpClient.GetAsync("api/salons");
                if (salonsResponse.IsSuccessStatusCode)
                {
                    var salonsContent = await salonsResponse.Content.ReadAsStringAsync();
                    var salonDtos = JsonConvert.DeserializeObject<List<SalonDto>>(salonsContent);
                    ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                else
                {
                    ViewBag.Salons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
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
                var response = await _httpClient.GetAsync($"api/sessions/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var sessionDto = JsonConvert.DeserializeObject<SessionDto>(content);
                    
                    if (sessionDto == null)
                        return NotFound();
                    
                    var sessionViewModel = new SessionViewModel
                    {
                        Id = sessionDto.Id,
                        MovieId = sessionDto.MovieId,
                        SalonId = sessionDto.SalonId,
                        StartTime = sessionDto.SessionDate.Date.Add(sessionDto.StartTime),
                        Price = sessionDto.Price,
                        IsActive = sessionDto.IsActive,
                        CreatedAt = sessionDto.CreatedDate,
                        MovieTitle = sessionDto.MovieTitle ?? string.Empty,
                        SalonName = sessionDto.SalonName ?? string.Empty
                    };

                    // Film listesini yükle
                    var moviesResponse = await _httpClient.GetAsync("api/movies");
                    if (moviesResponse.IsSuccessStatusCode)
                    {
                        var moviesContent = await moviesResponse.Content.ReadAsStringAsync();
                        var movieDtos = JsonConvert.DeserializeObject<List<MovieDto>>(moviesContent);
                        ViewBag.Movies = movieDtos?.Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = m.Id.ToString(),
                            Text = m.Title
                        }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                    }

                    // Salon listesini yükle
                    var salonsResponse = await _httpClient.GetAsync("api/salons");
                    if (salonsResponse.IsSuccessStatusCode)
                    {
                        var salonsContent = await salonsResponse.Content.ReadAsStringAsync();
                        var salonDtos = JsonConvert.DeserializeObject<List<SalonDto>>(salonsContent);
                        ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = s.Id.ToString(),
                            Text = s.Name
                        }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                    }
                    
                    return View(sessionViewModel);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return NotFound();
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
                        SessionDate = sessionViewModel.StartTime.Date,
                        StartTime = sessionViewModel.StartTime.TimeOfDay,
                        IsActive = sessionViewModel.IsActive,
                        Price = sessionViewModel.Price
                    };

                    var json = JsonConvert.SerializeObject(updateSessionDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"api/sessions/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Seans güncellenirken hata oluştu.");
                }
            }

            // Hata durumunda film ve salon listelerini tekrar yükle
            try
            {
                var moviesResponse = await _httpClient.GetAsync("api/movies");
                if (moviesResponse.IsSuccessStatusCode)
                {
                    var moviesContent = await moviesResponse.Content.ReadAsStringAsync();
                    var movieDtos = JsonConvert.DeserializeObject<List<MovieDto>>(moviesContent);
                    ViewBag.Movies = movieDtos?.Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Title
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                else
                {
                    ViewBag.Movies = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                var salonsResponse = await _httpClient.GetAsync("api/salons");
                if (salonsResponse.IsSuccessStatusCode)
                {
                    var salonsContent = await salonsResponse.Content.ReadAsStringAsync();
                    var salonDtos = JsonConvert.DeserializeObject<List<SalonDto>>(salonsContent);
                    ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                else
                {
                    ViewBag.Salons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
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
                var response = await _httpClient.DeleteAsync($"api/sessions/{id}?deletedBy=Admin&reason=Silme");
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
                var response = await _httpClient.GetAsync($"api/sessions/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var sessionDto = JsonConvert.DeserializeObject<SessionDto>(content);
                    
                    if (sessionDto == null)
                        return NotFound();
                    
                    var sessionViewModel = new SessionViewModel
                    {
                        Id = sessionDto.Id,
                        MovieId = sessionDto.MovieId,
                        SalonId = sessionDto.SalonId,
                        StartTime = sessionDto.SessionDate.Date.Add(sessionDto.StartTime),
                        Price = sessionDto.Price,
                        IsActive = sessionDto.IsActive,
                        CreatedAt = sessionDto.CreatedDate,
                        MovieTitle = sessionDto.MovieTitle ?? string.Empty,
                        SalonName = sessionDto.SalonName ?? string.Empty
                    };
                    
                    return View(sessionViewModel);
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