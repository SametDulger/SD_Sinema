using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using SD_Sinema.Web.Services;

namespace SD_Sinema.Web.Controllers
{
    public class TicketTypesController : Controller
    {
        private readonly IApiService _apiService;

        public TicketTypesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var ticketTypeDtos = await _apiService.GetAsync<List<TicketTypeDto>>("api/tickettypes");
                
                var ticketTypeViewModels = ticketTypeDtos?.Select(t => new TicketTypeViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description ?? string.Empty,
                    Price = t.Price,
                    DiscountPercentage = t.DiscountPercentage,
                    IsActive = t.IsActive,
                    CreatedAt = t.CreatedDate
                }).ToList() ?? new List<TicketTypeViewModel>();
                
                return View(ticketTypeViewModels);
            }
            catch
            {
                return View(new List<TicketTypeViewModel>());
            }
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
                        Price = ticketTypeViewModel.Price,
                        DiscountPercentage = ticketTypeViewModel.DiscountPercentage,
                        IsActive = ticketTypeViewModel.IsActive
                    };

                    var result = await _apiService.PostAsync<TicketTypeDto>("api/tickettypes", createTicketTypeDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Bilet türü oluşturulurken hata oluştu.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Bilet türü oluşturulurken hata oluştu.");
                }
            }

            return View(ticketTypeViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var ticketTypeDto = await _apiService.GetAsync<TicketTypeDto>($"api/tickettypes/{id}");
                
                if (ticketTypeDto == null)
                    return NotFound();
                
                var ticketTypeViewModel = new TicketTypeViewModel
                {
                    Id = ticketTypeDto.Id,
                    Name = ticketTypeDto.Name,
                    Description = ticketTypeDto.Description ?? string.Empty,
                    Price = ticketTypeDto.Price,
                    DiscountPercentage = ticketTypeDto.DiscountPercentage,
                    IsActive = ticketTypeDto.IsActive
                };

                return View(ticketTypeViewModel);
            }
            catch
            {
                return NotFound();
            }
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
                        Name = ticketTypeViewModel.Name,
                        Description = ticketTypeViewModel.Description,
                        Price = ticketTypeViewModel.Price,
                        DiscountPercentage = ticketTypeViewModel.DiscountPercentage,
                        IsActive = ticketTypeViewModel.IsActive
                    };

                    var result = await _apiService.PutAsync<TicketTypeDto>($"api/tickettypes/{id}", updateTicketTypeDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Bilet türü güncellenirken hata oluştu.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Bilet türü güncellenirken hata oluştu.");
                }
            }

            return View(ticketTypeViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ticketTypeDto = await _apiService.GetAsync<TicketTypeDto>($"api/tickettypes/{id}");
                
                if (ticketTypeDto == null)
                    return NotFound();
                
                var ticketTypeViewModel = new TicketTypeViewModel
                {
                    Id = ticketTypeDto.Id,
                    Name = ticketTypeDto.Name,
                    Description = ticketTypeDto.Description ?? string.Empty,
                    Price = ticketTypeDto.Price,
                    DiscountPercentage = ticketTypeDto.DiscountPercentage,
                    IsActive = ticketTypeDto.IsActive
                };

                return View(ticketTypeViewModel);
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
                var success = await _apiService.DeleteAsync($"api/tickettypes/{id}?deletedBy=Admin&reason=Silme");
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Bilet türü silinirken hata oluştu.");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Bilet türü silinirken hata oluştu.");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var ticketTypeDto = await _apiService.GetAsync<TicketTypeDto>($"api/tickettypes/{id}");
                
                if (ticketTypeDto == null)
                    return NotFound();
                
                var ticketTypeViewModel = new TicketTypeViewModel
                {
                    Id = ticketTypeDto.Id,
                    Name = ticketTypeDto.Name,
                    Description = ticketTypeDto.Description ?? string.Empty,
                    Price = ticketTypeDto.Price,
                    DiscountPercentage = ticketTypeDto.DiscountPercentage,
                    IsActive = ticketTypeDto.IsActive,
                    CreatedAt = ticketTypeDto.CreatedDate
                };

                return View(ticketTypeViewModel);
            }
            catch
            {
                return NotFound();
            }
        }
    }
} 