using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using SD_Sinema.Web.Services;

namespace SD_Sinema.Web.Controllers
{
    public class SeatsController : Controller
    {
        private readonly IApiService _apiService;

        public SeatsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var seatDtos = await _apiService.GetAsync<List<SeatDto>>("api/seats");
                
                var seatViewModels = seatDtos?.Select(s => new SeatViewModel
                {
                    Id = s.Id,
                    SalonId = s.SalonId,
                    SeatNumber = s.SeatNumber.ToString(),
                    RowNumber = int.Parse(s.RowNumber),
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedDate,
                    SalonName = s.SalonName
                }).ToList() ?? new List<SeatViewModel>();
                
                return View(seatViewModels);
            }
            catch
            {
                // API bağlantı hatası
                return View(new List<SeatViewModel>());
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                // Salon listesini yükle
                var salonDtos = await _apiService.GetAsync<List<SalonDto>>("api/salons");
                ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                // Koltuk tiplerini yükle
                var seatTypes = await _apiService.GetAsync<List<string>>("api/seats/types");
                ViewBag.SeatTypes = seatTypes?.Select(st => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = st,
                    Text = st
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Salons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.SeatTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
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
                        SeatTypeId = seatViewModel.SeatTypeId,
                        IsActive = seatViewModel.IsActive
                    };

                    var result = await _apiService.PostAsync<SeatDto>("api/seats", createSeatDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Koltuk oluşturulurken hata oluştu.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Koltuk oluşturulurken hata oluştu: {ex.Message}");
                }
            }

            // Hata durumunda dropdown'ları tekrar doldur
            try
            {
                var salonDtos = await _apiService.GetAsync<List<SalonDto>>("api/salons");
                ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                var seatTypes = await _apiService.GetAsync<List<string>>("api/seats/types");
                ViewBag.SeatTypes = seatTypes?.Select(st => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = st,
                    Text = st
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Salons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.SeatTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(seatViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var seatDto = await _apiService.GetAsync<SeatDto>($"api/seats/{id}");
                
                if (seatDto == null)
                    return NotFound();
                
                var seatViewModel = new SeatViewModel
                {
                    Id = seatDto.Id,
                    SalonId = seatDto.SalonId,
                    SeatNumber = seatDto.SeatNumber.ToString(),
                    RowNumber = int.Parse(seatDto.RowNumber),
                    IsActive = seatDto.IsActive,
                    CreatedAt = seatDto.CreatedDate,
                    SalonName = seatDto.SalonName
                };

                // Salon listesini yükle
                var salonDtos = await _apiService.GetAsync<List<SalonDto>>("api/salons");
                ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                // Koltuk tiplerini yükle
                var seatTypes = await _apiService.GetAsync<List<string>>("api/seats/types");
                ViewBag.SeatTypes = seatTypes?.Select(st => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = st,
                    Text = st
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                
                return View(seatViewModel);
            }
            catch
            {
                return NotFound();
            }
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
                        SeatTypeId = seatViewModel.SeatTypeId,
                        IsActive = seatViewModel.IsActive
                    };

                    var result = await _apiService.PutAsync<SeatDto>($"api/seats/{id}", updateSeatDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Koltuk güncellenirken hata oluştu.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Koltuk güncellenirken hata oluştu: {ex.Message}");
                }
            }

            // Hata durumunda dropdown'ları tekrar doldur
            try
            {
                var salonDtos = await _apiService.GetAsync<List<SalonDto>>("api/salons");
                ViewBag.Salons = salonDtos?.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                var seatTypes = await _apiService.GetAsync<List<string>>("api/seats/types");
                ViewBag.SeatTypes = seatTypes?.Select(st => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = st,
                    Text = st
                }).ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch
            {
                ViewBag.Salons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.SeatTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }

            return View(seatViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var seatDto = await _apiService.GetAsync<SeatDto>($"api/seats/{id}");
                
                if (seatDto == null)
                    return NotFound();
                
                var seatViewModel = new SeatViewModel
                {
                    Id = seatDto.Id,
                    SalonId = seatDto.SalonId,
                    SeatNumber = seatDto.SeatNumber.ToString(),
                    RowNumber = int.Parse(seatDto.RowNumber),
                    IsActive = seatDto.IsActive,
                    CreatedAt = seatDto.CreatedDate,
                    SalonName = seatDto.SalonName
                };
                
                return View(seatViewModel);
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
                var success = await _apiService.DeleteAsync($"api/seats/{id}");
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Koltuk silinirken hata oluştu.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Koltuk silinirken hata oluştu: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var seatDto = await _apiService.GetAsync<SeatDto>($"api/seats/{id}");
                
                if (seatDto == null)
                    return NotFound();
                
                var seatViewModel = new SeatViewModel
                {
                    Id = seatDto.Id,
                    SalonId = seatDto.SalonId,
                    SeatNumber = seatDto.SeatNumber.ToString(),
                    RowNumber = int.Parse(seatDto.RowNumber),
                    IsActive = seatDto.IsActive,
                    CreatedAt = seatDto.CreatedDate,
                    SalonName = seatDto.SalonName
                };
                
                return View(seatViewModel);
            }
            catch
            {
                return NotFound();
            }
        }
    }
} 