using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Web.Models;
using System.Text;

namespace SD_Sinema.Web.Controllers
{
    public class SalonsController : Controller
    {
        private readonly HttpClient _httpClient;

        public SalonsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/salons");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var salonDtos = JsonConvert.DeserializeObject<List<SalonDto>>(content);
                    
                    var salonViewModels = salonDtos?.Select(s => new SalonViewModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description ?? string.Empty,
                        Capacity = s.Capacity,
                        IsActive = s.IsActive,
                        CreatedAt = s.CreatedDate
                    }).ToList() ?? new List<SalonViewModel>();
                    
                    return View(salonViewModels);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return View(Enumerable.Empty<SalonViewModel>());
        }

        public IActionResult Create()
        {
            return View(new SalonViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SalonViewModel salonViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createSalonDto = new CreateSalonDto
                    {
                        Name = salonViewModel.Name,
                        Capacity = salonViewModel.Capacity,
                        Description = salonViewModel.Description
                    };

                    var json = JsonConvert.SerializeObject(createSalonDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/salons", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Salon oluşturulurken hata oluştu.");
                }
            }

            return View(salonViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/salons/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var salonDto = JsonConvert.DeserializeObject<SalonDto>(content);
                    
                    if (salonDto == null)
                        return NotFound();
                    
                    var salonViewModel = new SalonViewModel
                    {
                        Id = salonDto.Id,
                        Name = salonDto.Name,
                        Description = salonDto.Description ?? string.Empty,
                        Capacity = salonDto.Capacity,
                        IsActive = salonDto.IsActive,
                        CreatedAt = salonDto.CreatedDate
                    };
                    
                    return View(salonViewModel);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SalonViewModel salonViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateSalonDto = new UpdateSalonDto
                    {
                        Id = id,
                        Name = salonViewModel.Name,
                        Capacity = salonViewModel.Capacity,
                        Description = salonViewModel.Description
                    };

                    var json = JsonConvert.SerializeObject(updateSalonDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"api/salons/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Salon güncellenirken hata oluştu.");
                }
            }

            return View(salonViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/salons/{id}?deletedBy=Admin&reason=Silme");
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
                var response = await _httpClient.GetAsync($"api/salons/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var salonDto = JsonConvert.DeserializeObject<SalonDto>(content);
                    
                    if (salonDto == null)
                        return NotFound();
                    
                    var salonViewModel = new SalonViewModel
                    {
                        Id = salonDto.Id,
                        Name = salonDto.Name,
                        Description = salonDto.Description ?? string.Empty,
                        Capacity = salonDto.Capacity,
                        IsActive = salonDto.IsActive,
                        CreatedAt = salonDto.CreatedDate
                    };
                    
                    return View(salonViewModel);
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