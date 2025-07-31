using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using SD_Sinema.Web.Services;

namespace SD_Sinema.Web.Controllers
{
    public class SalonsController : Controller
    {
        private readonly IApiService _apiService;

        public SalonsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var salonDtos = await _apiService.GetAsync<List<SalonDto>>("api/salons");
                
                var salonViewModels = salonDtos?.Select(s => new SalonViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Capacity = s.Capacity,
                    Description = s.Description ?? string.Empty,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedDate
                }).ToList() ?? new List<SalonViewModel>();
                
                return View(salonViewModels);
            }
            catch
            {
                return View(new List<SalonViewModel>());
            }
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
                        Description = salonViewModel.Description,
                        IsActive = salonViewModel.IsActive
                    };

                    var result = await _apiService.PostAsync<SalonDto>("api/salons", createSalonDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Salon oluşturulurken hata oluştu.");
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
                var salonDto = await _apiService.GetAsync<SalonDto>($"api/salons/{id}");
                
                if (salonDto == null)
                    return NotFound();
                
                var salonViewModel = new SalonViewModel
                {
                    Id = salonDto.Id,
                    Name = salonDto.Name,
                    Capacity = salonDto.Capacity,
                    Description = salonDto.Description ?? string.Empty,
                    IsActive = salonDto.IsActive
                };

                return View(salonViewModel);
            }
            catch
            {
                return NotFound();
            }
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
                        Name = salonViewModel.Name,
                        Capacity = salonViewModel.Capacity,
                        Description = salonViewModel.Description,
                        IsActive = salonViewModel.IsActive
                    };

                    var result = await _apiService.PutAsync<SalonDto>($"api/salons/{id}", updateSalonDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Salon güncellenirken hata oluştu.");
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
                var salonDto = await _apiService.GetAsync<SalonDto>($"api/salons/{id}");
                
                if (salonDto == null)
                    return NotFound();
                
                var salonViewModel = new SalonViewModel
                {
                    Id = salonDto.Id,
                    Name = salonDto.Name,
                    Capacity = salonDto.Capacity,
                    Description = salonDto.Description ?? string.Empty,
                    IsActive = salonDto.IsActive
                };

                return View(salonViewModel);
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
                var success = await _apiService.DeleteAsync($"api/salons/{id}?deletedBy=Admin&reason=Silme");
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Salon silinirken hata oluştu.");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Salon silinirken hata oluştu.");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var salonDto = await _apiService.GetAsync<SalonDto>($"api/salons/{id}");
                
                if (salonDto == null)
                    return NotFound();
                
                var salonViewModel = new SalonViewModel
                {
                    Id = salonDto.Id,
                    Name = salonDto.Name,
                    Capacity = salonDto.Capacity,
                    Description = salonDto.Description ?? string.Empty,
                    IsActive = salonDto.IsActive,
                    CreatedAt = salonDto.CreatedDate
                };

                return View(salonViewModel);
            }
            catch
            {
                return NotFound();
            }
        }
    }
} 