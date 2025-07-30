using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using System.Text;

namespace SD_Sinema.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly HttpClient _httpClient;

        public TicketsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/tickets");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ticketDtos = JsonConvert.DeserializeObject<List<TicketDto>>(content);
                    
                    var ticketViewModels = ticketDtos?.Select(t => new SD_Sinema.Web.Models.TicketViewModel
                    {
                        Id = t.Id,
                        ReservationId = 0, // API'den gelmiyor, varsayılan değer
                        TicketTypeId = t.TicketTypeId,
                        Price = t.Price,
                        Status = t.Status,
                        CreatedAt = t.CreatedDate,
                        ReservationInfo = $"{t.MovieTitle} - {t.SalonName}",
                        TicketTypeName = t.TicketTypeName,
                        SeatInfo = t.SeatInfo
                    }).ToList() ?? new List<SD_Sinema.Web.Models.TicketViewModel>();
                    
                    return View(ticketViewModels);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return View(Enumerable.Empty<TicketViewModel>());
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                // Rezervasyon listesini yükle
                var reservationsResponse = await _httpClient.GetAsync("api/reservations");
                if (reservationsResponse.IsSuccessStatusCode)
                {
                    var reservationsContent = await reservationsResponse.Content.ReadAsStringAsync();
                    var reservationDtos = JsonConvert.DeserializeObject<List<ReservationDto>>(reservationsContent);
                    ViewBag.Reservations = reservationDtos?.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"{r.UserName} - {r.MovieTitle}"
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                // Bilet türü listesini yükle
                var ticketTypesResponse = await _httpClient.GetAsync("api/tickettypes");
                if (ticketTypesResponse.IsSuccessStatusCode)
                {
                    var ticketTypesContent = await ticketTypesResponse.Content.ReadAsStringAsync();
                    var ticketTypeDtos = JsonConvert.DeserializeObject<List<TicketTypeDto>>(ticketTypesContent);
                    ViewBag.TicketTypes = ticketTypeDtos?.Select(t => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Name
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
                        TicketNumber = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                        Price = ticketViewModel.Price,
                        PurchaseDate = DateTime.Now,
                        Status = "Active"
                    };

                    var json = JsonConvert.SerializeObject(createTicketDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/tickets", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Bilet oluşturulurken hata oluştu.");
                }
            }

            // Hata durumunda dropdown listelerini tekrar yükle
            try
            {
                var reservationsResponse = await _httpClient.GetAsync("api/reservations");
                if (reservationsResponse.IsSuccessStatusCode)
                {
                    var reservationsContent = await reservationsResponse.Content.ReadAsStringAsync();
                    var reservationDtos = JsonConvert.DeserializeObject<List<ReservationDto>>(reservationsContent);
                    ViewBag.Reservations = reservationDtos?.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"{r.UserName} - {r.MovieTitle}"
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                else
                {
                    ViewBag.Reservations = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                var ticketTypesResponse = await _httpClient.GetAsync("api/tickettypes");
                if (ticketTypesResponse.IsSuccessStatusCode)
                {
                    var ticketTypesContent = await ticketTypesResponse.Content.ReadAsStringAsync();
                    var ticketTypeDtos = JsonConvert.DeserializeObject<List<TicketTypeDto>>(ticketTypesContent);
                    ViewBag.TicketTypes = ticketTypeDtos?.Select(t => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Name
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
                else
                {
                    ViewBag.TicketTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

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
                else
                {
                    ViewBag.Seats = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
            }
            catch
            {
                ViewBag.Reservations = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.TicketTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Seats = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(ticketViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/tickets/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ticketDto = JsonConvert.DeserializeObject<TicketDto>(content);
                    
                    if (ticketDto == null)
                        return NotFound();
                    
                    var ticketViewModel = new TicketViewModel
                    {
                        Id = ticketDto.Id,
                        ReservationId = 0,
                        TicketTypeId = ticketDto.TicketTypeId,
                        SeatId = 0,
                        Price = ticketDto.Price,
                        Status = ticketDto.Status,
                        CreatedAt = ticketDto.CreatedDate,
                        ReservationInfo = $"{(ticketDto.MovieTitle ?? string.Empty)} - {(ticketDto.SalonName ?? string.Empty)}",
                        TicketTypeName = ticketDto.TicketTypeName ?? string.Empty,
                        SeatInfo = ticketDto.SeatInfo ?? string.Empty
                    };

                    // Rezervasyon listesini yükle
                    var reservationsResponse = await _httpClient.GetAsync("api/reservations");
                    if (reservationsResponse.IsSuccessStatusCode)
                    {
                        var reservationsContent = await reservationsResponse.Content.ReadAsStringAsync();
                        var reservationDtos = JsonConvert.DeserializeObject<List<ReservationDto>>(reservationsContent);
                        ViewBag.Reservations = reservationDtos?.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = r.Id.ToString(),
                            Text = $"{r.UserName} - {r.MovieTitle}"
                        }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                    }

                    // Bilet türü listesini yükle
                    var ticketTypesResponse = await _httpClient.GetAsync("api/tickettypes");
                    if (ticketTypesResponse.IsSuccessStatusCode)
                    {
                        var ticketTypesContent = await ticketTypesResponse.Content.ReadAsStringAsync();
                        var ticketTypeDtos = JsonConvert.DeserializeObject<List<TicketTypeDto>>(ticketTypesContent);
                        ViewBag.TicketTypes = ticketTypeDtos?.Select(t => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = t.Id.ToString(),
                            Text = t.Name
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
                    
                    return View(ticketViewModel);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return NotFound();
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
                        Id = id,
                        ReservationId = ticketViewModel.ReservationId,
                        TicketNumber = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                        Price = ticketViewModel.Price,
                        PurchaseDate = DateTime.Now,
                        Status = ticketViewModel.Status
                    };

                    var json = JsonConvert.SerializeObject(updateTicketDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"api/tickets/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Bilet güncellenirken hata oluştu.");
                }
            }

            // Hata durumunda dropdown listelerini tekrar yükle
            try
            {
                var reservationsResponse = await _httpClient.GetAsync("api/reservations");
                if (reservationsResponse.IsSuccessStatusCode)
                {
                    var reservationsContent = await reservationsResponse.Content.ReadAsStringAsync();
                    var reservationDtos = JsonConvert.DeserializeObject<List<ReservationDto>>(reservationsContent);
                    ViewBag.Reservations = reservationDtos?.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"{r.UserName} - {r.MovieTitle}"
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

                var ticketTypesResponse = await _httpClient.GetAsync("api/tickettypes");
                if (ticketTypesResponse.IsSuccessStatusCode)
                {
                    var ticketTypesContent = await ticketTypesResponse.Content.ReadAsStringAsync();
                    var ticketTypeDtos = JsonConvert.DeserializeObject<List<TicketTypeDto>>(ticketTypesContent);
                    ViewBag.TicketTypes = ticketTypeDtos?.Select(t => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Name
                    }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }

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
                ViewBag.Reservations = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.TicketTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Seats = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(ticketViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/tickets/{id}?deletedBy=Admin&reason=Silme");
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