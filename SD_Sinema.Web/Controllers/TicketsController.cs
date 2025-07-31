using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using SD_Sinema.Web.Services;

namespace SD_Sinema.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IApiService _apiService;

        public TicketsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var ticketDtos = await _apiService.GetAsync<List<TicketDto>>("api/tickets");
                
                var ticketViewModels = ticketDtos?.Select(t => new TicketViewModel
                {
                    Id = t.Id,
                    ReservationId = t.ReservationId,
                    TicketTypeId = t.TicketTypeId,
                    SeatId = t.SeatId,
                    Price = t.Price,
                    Status = t.Status,
                    CreatedAt = t.CreatedDate,
                    ReservationInfo = $"{t.MovieTitle} - {t.SalonName}",
                    TicketTypeName = t.TicketTypeName,
                    SeatInfo = t.SeatInfo
                }).ToList() ?? new List<TicketViewModel>();
                
                return View(ticketViewModels);
            }
            catch
            {
                return View(new List<TicketViewModel>());
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                // Rezervasyon listesini yükle
                var reservationDtos = await _apiService.GetAsync<List<ReservationDto>>("api/reservations");
                ViewBag.Reservations = reservationDtos?.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"{r.UserName} - {r.MovieTitle}"
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                // Bilet türü listesini yükle
                var ticketTypeDtos = await _apiService.GetAsync<List<TicketTypeDto>>("api/tickettypes");
                ViewBag.TicketTypes = ticketTypeDtos?.Select(t => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
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
                ViewBag.Reservations = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.TicketTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Seats = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(new TicketViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketViewModel ticketViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createTicketDto = new CreateTicketDto
                    {
                        ReservationId = ticketViewModel.ReservationId,
                        TicketTypeId = ticketViewModel.TicketTypeId,
                        SeatId = ticketViewModel.SeatId,
                        Price = ticketViewModel.Price,
                        Status = ticketViewModel.Status,
                        TicketNumber = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                        PurchaseDate = DateTime.Now
                    };

                    var result = await _apiService.PostAsync<TicketDto>("api/tickets", createTicketDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Bilet oluşturulurken hata oluştu.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Bilet oluşturulurken hata oluştu.");
                }
            }

            // Hata durumunda dropdown'ları tekrar doldur
            try
            {
                var reservationDtos = await _apiService.GetAsync<List<ReservationDto>>("api/reservations");
                ViewBag.Reservations = reservationDtos?.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"{r.UserName} - {r.MovieTitle}"
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Reservations = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            try
            {
                var ticketTypeDtos = await _apiService.GetAsync<List<TicketTypeDto>>("api/tickettypes");
                ViewBag.TicketTypes = ticketTypeDtos?.Select(t => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.TicketTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
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

            return View(ticketViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var ticketDto = await _apiService.GetAsync<TicketDto>($"api/tickets/{id}");
                
                if (ticketDto == null)
                    return NotFound();
                
                var ticketViewModel = new TicketViewModel
                {
                    Id = ticketDto.Id,
                    ReservationId = ticketDto.ReservationId,
                    TicketTypeId = ticketDto.TicketTypeId,
                    SeatId = ticketDto.SeatId,
                    Price = ticketDto.Price,
                    Status = ticketDto.Status,
                    ReservationInfo = $"{(ticketDto.MovieTitle ?? string.Empty)} - {(ticketDto.SalonName ?? string.Empty)}",
                    TicketTypeName = ticketDto.TicketTypeName ?? string.Empty,
                    SeatInfo = ticketDto.SeatInfo ?? string.Empty
                };

                // Dropdown'ları doldur
                try
                {
                    var reservationDtos = await _apiService.GetAsync<List<ReservationDto>>("api/reservations");
                    ViewBag.Reservations = reservationDtos?.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"{r.UserName} - {r.MovieTitle}"
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                catch
                {
                    ViewBag.Reservations = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                try
                {
                    var ticketTypeDtos = await _apiService.GetAsync<List<TicketTypeDto>>("api/tickettypes");
                    ViewBag.TicketTypes = ticketTypeDtos?.Select(t => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Name
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                catch
                {
                    ViewBag.TicketTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
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

                return View(ticketViewModel);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TicketViewModel ticketViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateTicketDto = new UpdateTicketDto
                    {
                        ReservationId = ticketViewModel.ReservationId,
                        TicketTypeId = ticketViewModel.TicketTypeId,
                        SeatId = ticketViewModel.SeatId,
                        Price = ticketViewModel.Price,
                        Status = ticketViewModel.Status
                    };

                    var result = await _apiService.PutAsync<TicketDto>($"api/tickets/{id}", updateTicketDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Bilet güncellenirken hata oluştu.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Bilet güncellenirken hata oluştu.");
                }
            }

            // Hata durumunda dropdown'ları tekrar doldur
            try
            {
                var reservationDtos = await _apiService.GetAsync<List<ReservationDto>>("api/reservations");
                ViewBag.Reservations = reservationDtos?.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"{r.UserName} - {r.MovieTitle}"
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Reservations = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            try
            {
                var ticketTypeDtos = await _apiService.GetAsync<List<TicketTypeDto>>("api/tickettypes");
                ViewBag.TicketTypes = ticketTypeDtos?.Select(t => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.TicketTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
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

            return View(ticketViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _apiService.DeleteAsync($"api/tickets/{id}?deletedBy=Admin&reason=Silme");
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Bilet silinirken hata oluştu.");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Bilet silinirken hata oluştu.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 