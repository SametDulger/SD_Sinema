using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Web.Models;
using System.Text;

namespace SD_Sinema.Web.Controllers
{
    public class SeatsController : Controller
    {
        private readonly HttpClient _httpClient;

        public SeatsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/seats");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var seatDtos = JsonConvert.DeserializeObject<List<SeatDto>>(content);
                    
                    var seatViewModels = seatDtos?.Select(s => new SeatViewModel
                    {
                        Id = s.Id,
                        SalonId = s.SalonId,
                        SeatNumber = $"{s.RowNumber}{s.SeatNumber}",
                        RowNumber = s.SeatNumber,
                        IsActive = s.IsActive,
                        CreatedAt = s.CreatedDate,
                        SalonName = s.SalonName
                    }).ToList() ?? new List<SeatViewModel>();
                    
                    return View(seatViewModels);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return View(Enumerable.Empty<SeatViewModel>());
        }

        public async Task<IActionResult> Create()
        {
            try
            {
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
                ViewBag.Salons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(new SeatViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SeatViewModel seatViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createSeatDto = new CreateSeatDto
                    {
                        SalonId = seatViewModel.SalonId,
                        SeatNumber = seatViewModel.SeatNumber,
                        RowNumber = seatViewModel.RowNumber,
                        SeatType = "Normal"
                    };

                    var json = JsonConvert.SerializeObject(createSeatDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/seats", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Koltuk oluşturulurken hata oluştu.");
                }
            }

            // Hata durumunda salon listesini tekrar yükle
            try
            {
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
                ViewBag.Salons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(seatViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/seats/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var seatDto = JsonConvert.DeserializeObject<SeatDto>(content);
                    
                    if (seatDto == null)
                        return NotFound();
                    
                    var seatViewModel = new SeatViewModel
                    {
                        Id = seatDto.Id,
                        SalonId = seatDto.SalonId,
                        SeatNumber = seatDto.RowNumber,
                        RowNumber = seatDto.SeatNumber,
                        IsActive = seatDto.IsActive,
                        CreatedAt = seatDto.CreatedDate,
                        SalonName = seatDto.SalonName
                    };

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
                    
                    return View(seatViewModel);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SeatViewModel seatViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateSeatDto = new UpdateSeatDto
                    {
                        Id = id,
                        SalonId = seatViewModel.SalonId,
                        SeatNumber = seatViewModel.SeatNumber,
                        RowNumber = seatViewModel.RowNumber,
                        SeatType = "Normal"
                    };

                    var json = JsonConvert.SerializeObject(updateSeatDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"api/seats/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Koltuk güncellenirken hata oluştu.");
                }
            }

            // Hata durumunda salon listesini tekrar yükle
            try
            {
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
                ViewBag.Salons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(seatViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/seats/{id}?deletedBy=Admin&reason=Silme");
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