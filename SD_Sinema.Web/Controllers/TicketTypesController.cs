using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Web.Models;
using System.Text;

namespace SD_Sinema.Web.Controllers
{
    public class TicketTypesController : Controller
    {
        private readonly HttpClient _httpClient;

        public TicketTypesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/tickettypes");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ticketTypeDtos = JsonConvert.DeserializeObject<List<TicketTypeDto>>(content);
                    
                    var ticketTypeViewModels = ticketTypeDtos?.Select(t => new TicketTypeViewModel
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description ?? string.Empty,
                        Price = t.Price,
                        IsActive = t.IsActive,
                        CreatedAt = t.CreatedDate
                    }).ToList() ?? new List<TicketTypeViewModel>();
                    
                    return View(ticketTypeViewModels);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return View(Enumerable.Empty<TicketTypeViewModel>());
        }

        public IActionResult Create()
        {
            return View(new TicketTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketTypeViewModel ticketTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createTicketTypeDto = new CreateTicketTypeDto
                    {
                        Name = ticketTypeViewModel.Name,
                        Description = ticketTypeViewModel.Description,
                        Price = ticketTypeViewModel.Price
                    };

                    var json = JsonConvert.SerializeObject(createTicketTypeDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/tickettypes", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Bilet tipi oluşturulurken hata oluştu.");
                }
            }

            return View(ticketTypeViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/tickettypes/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ticketTypeDto = JsonConvert.DeserializeObject<TicketTypeDto>(content);
                    
                    if (ticketTypeDto == null)
                        return NotFound();
                    
                    var ticketTypeViewModel = new TicketTypeViewModel
                    {
                        Id = ticketTypeDto.Id,
                        Name = ticketTypeDto.Name,
                        Description = ticketTypeDto.Description ?? string.Empty,
                        Price = ticketTypeDto.Price,
                        IsActive = ticketTypeDto.IsActive,
                        CreatedAt = ticketTypeDto.CreatedDate
                    };
                    
                    return View(ticketTypeViewModel);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TicketTypeViewModel ticketTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateTicketTypeDto = new UpdateTicketTypeDto
                    {
                        Id = id,
                        Name = ticketTypeViewModel.Name,
                        Description = ticketTypeViewModel.Description,
                        Price = ticketTypeViewModel.Price
                    };

                    var json = JsonConvert.SerializeObject(updateTicketTypeDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"api/tickettypes/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Bilet tipi güncellenirken hata oluştu.");
                }
            }

            return View(ticketTypeViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/tickettypes/{id}?deletedBy=Admin&reason=Silme");
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
                var response = await _httpClient.GetAsync($"api/tickettypes/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ticketTypeDto = JsonConvert.DeserializeObject<TicketTypeDto>(content);
                    
                    if (ticketTypeDto == null)
                        return NotFound();
                    
                    var ticketTypeViewModel = new TicketTypeViewModel
                    {
                        Id = ticketTypeDto.Id,
                        Name = ticketTypeDto.Name,
                        Description = ticketTypeDto.Description ?? string.Empty,
                        Price = ticketTypeDto.Price,
                        IsActive = ticketTypeDto.IsActive,
                        CreatedAt = ticketTypeDto.CreatedDate
                    };
                    
                    return View(ticketTypeViewModel);
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