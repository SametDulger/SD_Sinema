using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using System.Text;

namespace SD_Sinema.Web.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReservationsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/reservations");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var reservationDtos = JsonConvert.DeserializeObject<List<ReservationDto>>(content);
                    
                    var reservationViewModels = reservationDtos?.Select(r => new ReservationViewModel
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        SessionId = r.SessionId,
                        ReservationDate = r.ReservationDate,
                        SeatCount = 1, // API'den gelmiyor, varsayılan değer
                        TotalPrice = r.Price ?? 0,
                        Status = r.Status,
                        CreatedAt = r.CreatedDate,
                        UserName = r.UserName,
                        SessionInfo = $"{r.MovieTitle} - {r.SalonName}"
                    }).ToList() ?? new List<ReservationViewModel>();
                    
                    return View(reservationViewModels);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return View(Enumerable.Empty<ReservationViewModel>());
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                // Kullanıcı listesini yükle
                var usersResponse = await _httpClient.GetAsync("api/users");
                if (usersResponse.IsSuccessStatusCode)
                {
                    var usersContent = await usersResponse.Content.ReadAsStringAsync();
                    var userDtos = JsonConvert.DeserializeObject<List<UserDto>>(usersContent);
                    ViewBag.Users = userDtos?.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = $"{u.FirstName} {u.LastName}"
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                // Seans listesini yükle
                var sessionsResponse = await _httpClient.GetAsync("api/sessions");
                if (sessionsResponse.IsSuccessStatusCode)
                {
                    var sessionsContent = await sessionsResponse.Content.ReadAsStringAsync();
                    var sessionDtos = JsonConvert.DeserializeObject<List<SessionDto>>(sessionsContent);
                    ViewBag.Sessions = sessionDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.MovieTitle} - {s.SalonName}"
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                // Koltuk listesini yükle
                var seatsResponse = await _httpClient.GetAsync("api/seats");
                if (seatsResponse.IsSuccessStatusCode)
                {
                    var seatsContent = await seatsResponse.Content.ReadAsStringAsync();
                    var seatDtos = JsonConvert.DeserializeObject<List<SeatDto>>(seatsContent);
                    ViewBag.Seats = seatDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.SalonName} - {s.RowNumber}{s.SeatNumber}"
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
            }
            catch
            {
                ViewBag.Users = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Sessions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Seats = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(new ReservationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel reservationViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createReservationDto = new CreateReservationDto
                    {
                        UserId = reservationViewModel.UserId,
                        SessionId = reservationViewModel.SessionId,
                        SeatId = reservationViewModel.SeatId,
                        TicketTypeId = 1, // Varsayılan değer
                        ReservationDate = reservationViewModel.ReservationDate,
                        Status = "Pending",
                        Price = reservationViewModel.TotalPrice
                    };

                    var json = JsonConvert.SerializeObject(createReservationDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/reservations", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", $"API Hatası: {response.StatusCode} - {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Rezervasyon oluşturulurken hata oluştu: {ex.Message}");
                }
            }

            // Hata durumunda dropdown listelerini tekrar yükle ve seçilen değerleri koru
            try
            {
                var usersResponse = await _httpClient.GetAsync("api/users");
                if (usersResponse.IsSuccessStatusCode)
                {
                    var usersContent = await usersResponse.Content.ReadAsStringAsync();
                    var userDtos = JsonConvert.DeserializeObject<List<UserDto>>(usersContent);
                    ViewBag.Users = userDtos?.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = $"{u.FirstName} {u.LastName}",
                        Selected = u.Id == reservationViewModel.UserId
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                else
                {
                    ViewBag.Users = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                var sessionsResponse = await _httpClient.GetAsync("api/sessions");
                if (sessionsResponse.IsSuccessStatusCode)
                {
                    var sessionsContent = await sessionsResponse.Content.ReadAsStringAsync();
                    var sessionDtos = JsonConvert.DeserializeObject<List<SessionDto>>(sessionsContent);
                    ViewBag.Sessions = sessionDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.MovieTitle} - {s.SalonName}",
                        Selected = s.Id == reservationViewModel.SessionId
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                else
                {
                    ViewBag.Sessions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                var seatsResponse = await _httpClient.GetAsync("api/seats");
                if (seatsResponse.IsSuccessStatusCode)
                {
                    var seatsContent = await seatsResponse.Content.ReadAsStringAsync();
                    var seatDtos = JsonConvert.DeserializeObject<List<SeatDto>>(seatsContent);
                    ViewBag.Seats = seatDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.SalonName} - {s.RowNumber}{s.SeatNumber}",
                        Selected = s.Id == reservationViewModel.SeatId
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                else
                {
                    ViewBag.Seats = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
            }
            catch
            {
                ViewBag.Users = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Sessions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Seats = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(reservationViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/reservations/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var reservationDto = JsonConvert.DeserializeObject<ReservationDto>(content);
                    
                    if (reservationDto == null)
                        return NotFound();
                    
                    var reservationViewModel = new ReservationViewModel
                    {
                        Id = reservationDto.Id,
                        UserId = reservationDto.UserId,
                        SessionId = reservationDto.SessionId,
                        ReservationDate = reservationDto.ReservationDate,
                        SeatCount = 1,
                        TotalPrice = reservationDto.Price ?? 0,
                        Status = reservationDto.Status,
                        CreatedAt = reservationDto.CreatedDate,
                        UserName = reservationDto.UserName ?? string.Empty,
                        SessionInfo = $"{(reservationDto.MovieTitle ?? string.Empty)} - {(reservationDto.SalonName ?? string.Empty)}"
                    };

                    // Kullanıcı listesini yükle
                    var usersResponse = await _httpClient.GetAsync("api/users");
                    if (usersResponse.IsSuccessStatusCode)
                    {
                        var usersContent = await usersResponse.Content.ReadAsStringAsync();
                        var userDtos = JsonConvert.DeserializeObject<List<UserDto>>(usersContent);
                        ViewBag.Users = userDtos?.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = u.Id.ToString(),
                            Text = $"{u.FirstName} {u.LastName}"
                        }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                    }

                    // Seans listesini yükle
                    var sessionsResponse = await _httpClient.GetAsync("api/sessions");
                    if (sessionsResponse.IsSuccessStatusCode)
                    {
                        var sessionsContent = await sessionsResponse.Content.ReadAsStringAsync();
                        var sessionDtos = JsonConvert.DeserializeObject<List<SessionDto>>(sessionsContent);
                        ViewBag.Sessions = sessionDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = s.Id.ToString(),
                            Text = $"{s.MovieTitle} - {s.SalonName}"
                        }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                    }
                    
                    return View(reservationViewModel);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ReservationViewModel reservationViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateReservationDto = new UpdateReservationDto
                    {
                        Id = id,
                        UserId = reservationViewModel.UserId,
                        SessionId = reservationViewModel.SessionId,
                        SeatId = 1,
                        TicketTypeId = 1,
                        ReservationDate = reservationViewModel.ReservationDate,
                        Status = reservationViewModel.Status
                    };

                    var json = JsonConvert.SerializeObject(updateReservationDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"api/reservations/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Rezervasyon güncellenirken hata oluştu.");
                }
            }

            // Hata durumunda dropdown listelerini tekrar yükle
            try
            {
                var usersResponse = await _httpClient.GetAsync("api/users");
                if (usersResponse.IsSuccessStatusCode)
                {
                    var usersContent = await usersResponse.Content.ReadAsStringAsync();
                    var userDtos = JsonConvert.DeserializeObject<List<UserDto>>(usersContent);
                    ViewBag.Users = userDtos?.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = $"{u.FirstName} {u.LastName}"
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                var sessionsResponse = await _httpClient.GetAsync("api/sessions");
                if (sessionsResponse.IsSuccessStatusCode)
                {
                    var sessionsContent = await sessionsResponse.Content.ReadAsStringAsync();
                    var sessionDtos = JsonConvert.DeserializeObject<List<SessionDto>>(sessionsContent);
                    ViewBag.Sessions = sessionDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.MovieTitle} - {s.SalonName}"
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
            }
            catch
            {
                ViewBag.Users = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Sessions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(reservationViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/reservations/{id}?deletedBy=Admin&reason=Silme");
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
                var response = await _httpClient.GetAsync($"api/reservations/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var reservationDto = JsonConvert.DeserializeObject<ReservationDto>(content);
                    
                    if (reservationDto == null)
                        return NotFound();
                    
                    var reservationViewModel = new ReservationViewModel
                    {
                        Id = reservationDto.Id,
                        UserId = reservationDto.UserId,
                        SessionId = reservationDto.SessionId,
                        ReservationDate = reservationDto.ReservationDate,
                        SeatCount = 1, // API'den gelmiyor, varsayılan değer
                        TotalPrice = reservationDto.Price ?? 0,
                        Status = reservationDto.Status,
                        CreatedAt = reservationDto.CreatedDate,
                        UserName = reservationDto.UserName ?? string.Empty,
                        SessionInfo = $"{(reservationDto.MovieTitle ?? string.Empty)} - {(reservationDto.SalonName ?? string.Empty)}"
                    };
                    
                    return View(reservationViewModel);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return NotFound();
        }

        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/reservations/{id}/approve", null);
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

        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/reservations/{id}/cancel", null);
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
    }
} 