using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using SD_Sinema.Web.Services;

namespace SD_Sinema.Web.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IApiService _apiService;

        public ReservationsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var reservationDtos = await _apiService.GetAsync<List<ReservationDto>>("api/reservations");
                
                var reservationViewModels = reservationDtos?.Select(r => new ReservationViewModel
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    SessionId = r.SessionId,
                    SeatId = r.SeatId,
                    ReservationDate = r.ReservationDate,
                    SeatCount = r.SeatCount,
                    TotalPrice = r.TotalPrice,
                    Status = r.Status,
                    CreatedAt = r.CreatedDate,
                    UserName = r.UserName,
                    SessionInfo = r.SessionInfo
                }).ToList() ?? new List<ReservationViewModel>();
                
                return View(reservationViewModels);
            }
            catch
            {
                return View(new List<ReservationViewModel>());
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                // Kullanıcı listesini yükle
                var userDtos = await _apiService.GetAsync<List<UserDto>>("api/users");
                ViewBag.Users = userDtos?.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.FirstName} {u.LastName}"
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                // Seans listesini yükle
                var sessionDtos = await _apiService.GetAsync<List<SessionDto>>("api/sessions");
                ViewBag.Sessions = sessionDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.MovieTitle} - {s.SalonName} - {s.SessionDate:dd/MM/yyyy HH:mm}"
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                // Koltuk listesini yükle
                var seatDtos = await _apiService.GetAsync<List<SeatDto>>("api/seats");
                ViewBag.Seats = seatDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.SalonName} - {s.RowNumber}{s.SeatNumber}"
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
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
                        ReservationDate = reservationViewModel.ReservationDate,
                        SeatCount = reservationViewModel.SeatCount,
                        TotalPrice = reservationViewModel.TotalPrice,
                        Status = reservationViewModel.Status
                    };

                    var result = await _apiService.PostAsync<ReservationDto>("api/reservations", createReservationDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Rezervasyon oluşturulurken hata oluştu.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Rezervasyon oluşturulurken hata oluştu.");
                }
            }

            // Hata durumunda dropdown'ları tekrar doldur
            try
            {
                var userDtos = await _apiService.GetAsync<List<UserDto>>("api/users");
                ViewBag.Users = userDtos?.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.FirstName} {u.LastName}"
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Users = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            try
            {
                var sessionDtos = await _apiService.GetAsync<List<SessionDto>>("api/sessions");
                ViewBag.Sessions = sessionDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.MovieTitle} - {s.SalonName} - {s.SessionDate:dd/MM/yyyy HH:mm}"
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Sessions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            try
            {
                var seatDtos = await _apiService.GetAsync<List<SeatDto>>("api/seats");
                ViewBag.Seats = seatDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.SalonName} - {s.RowNumber}{s.SeatNumber}"
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Seats = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(reservationViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var reservationDto = await _apiService.GetAsync<ReservationDto>($"api/reservations/{id}");
                
                if (reservationDto == null)
                    return NotFound();
                
                var reservationViewModel = new ReservationViewModel
                {
                    Id = reservationDto.Id,
                    UserId = reservationDto.UserId,
                    SessionId = reservationDto.SessionId,
                    SeatId = reservationDto.SeatId,
                    ReservationDate = reservationDto.ReservationDate,
                    SeatCount = reservationDto.SeatCount,
                    TotalPrice = reservationDto.TotalPrice,
                    Status = reservationDto.Status,
                    UserName = reservationDto.UserName,
                    SessionInfo = reservationDto.SessionInfo
                };

                // Dropdown'ları doldur
                try
                {
                    var userDtos = await _apiService.GetAsync<List<UserDto>>("api/users");
                    ViewBag.Users = userDtos?.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = $"{u.FirstName} {u.LastName}"
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                catch
                {
                    ViewBag.Users = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                try
                {
                    var sessionDtos = await _apiService.GetAsync<List<SessionDto>>("api/sessions");
                    ViewBag.Sessions = sessionDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.MovieTitle} - {s.SalonName} - {s.SessionDate:dd/MM/yyyy HH:mm}"
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                catch
                {
                    ViewBag.Sessions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                return View(reservationViewModel);
            }
            catch
            {
                return NotFound();
            }
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
                        UserId = reservationViewModel.UserId,
                        SessionId = reservationViewModel.SessionId,
                        SeatId = reservationViewModel.SeatId,
                        ReservationDate = reservationViewModel.ReservationDate,
                        SeatCount = reservationViewModel.SeatCount,
                        TotalPrice = reservationViewModel.TotalPrice,
                        Status = reservationViewModel.Status
                    };

                    var result = await _apiService.PutAsync<ReservationDto>($"api/reservations/{id}", updateReservationDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Rezervasyon güncellenirken hata oluştu.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Rezervasyon güncellenirken hata oluştu.");
                }
            }

            // Hata durumunda dropdown'ları tekrar doldur
            try
            {
                var userDtos = await _apiService.GetAsync<List<UserDto>>("api/users");
                ViewBag.Users = userDtos?.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.FirstName} {u.LastName}"
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Users = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            try
            {
                var sessionDtos = await _apiService.GetAsync<List<SessionDto>>("api/sessions");
                ViewBag.Sessions = sessionDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.MovieTitle} - {s.SalonName} - {s.SessionDate:dd/MM/yyyy HH:mm}"
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Sessions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(reservationViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _apiService.DeleteAsync($"api/reservations/{id}?deletedBy=Admin&reason=Silme");
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Rezervasyon silinirken hata oluştu.");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Rezervasyon silinirken hata oluştu.");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var reservationDto = await _apiService.GetAsync<ReservationDto>($"api/reservations/{id}");
                
                if (reservationDto == null)
                    return NotFound();
                
                var reservationViewModel = new ReservationViewModel
                {
                    Id = reservationDto.Id,
                    UserId = reservationDto.UserId,
                    SessionId = reservationDto.SessionId,
                    SeatId = reservationDto.SeatId,
                    ReservationDate = reservationDto.ReservationDate,
                    SeatCount = reservationDto.SeatCount,
                    TotalPrice = reservationDto.TotalPrice,
                    Status = reservationDto.Status,
                    CreatedAt = reservationDto.CreatedDate,
                    UserName = reservationDto.UserName,
                    SessionInfo = reservationDto.SessionInfo
                };

                return View(reservationViewModel);
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var result = await _apiService.PostAsync<ReservationDto>($"api/reservations/{id}/approve", new { });
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Rezervasyon onaylanırken hata oluştu.");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Rezervasyon onaylanırken hata oluştu.");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var result = await _apiService.PostAsync<ReservationDto>($"api/reservations/{id}/cancel", new { });
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Rezervasyon iptal edilirken hata oluştu.");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Rezervasyon iptal edilirken hata oluştu.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 