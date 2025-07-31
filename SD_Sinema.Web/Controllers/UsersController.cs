using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using SD_Sinema.Web.Services;
using SD_Sinema.Web.Filters;

namespace SD_Sinema.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IApiService _apiService;

        public UsersController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userDtos = await _apiService.GetAsync<List<UserDto>>("api/users");
                
                var userViewModels = userDtos?.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Phone = u.PhoneNumber ?? string.Empty,
                    CreatedAt = u.CreatedDate
                }).ToList() ?? new List<UserViewModel>();
                
                return View(userViewModels);
            }
            catch
            {
                return View(new List<UserViewModel>());
            }
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createUserDto = new CreateUserDto
                    {
                        FirstName = userViewModel.FirstName,
                        LastName = userViewModel.LastName,
                        Email = userViewModel.Email,
                        Password = userViewModel.Password ?? "",
                        Phone = userViewModel.Phone
                    };

                    var result = await _apiService.PostAsync<UserDto>("api/users", createUserDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı oluşturulurken hata oluştu.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Kullanıcı oluşturulurken hata oluştu.");
                }
            }

            return View(userViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var userDto = await _apiService.GetAsync<UserDto>($"api/users/{id}");
                
                if (userDto == null)
                    return NotFound();
                
                var userViewModel = new UserViewModel
                {
                    Id = userDto.Id,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Phone = userDto.PhoneNumber ?? string.Empty
                };

                return View(userViewModel);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateUserDto = new UpdateUserDto
                    {
                        FirstName = userViewModel.FirstName,
                        LastName = userViewModel.LastName,
                        Email = userViewModel.Email,
                        Phone = userViewModel.Phone
                    };

                    var result = await _apiService.PutAsync<UserDto>($"api/users/{id}", updateUserDto);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı güncellenirken hata oluştu.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Kullanıcı güncellenirken hata oluştu.");
                }
            }

            return View(userViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userDto = await _apiService.GetAsync<UserDto>($"api/users/{id}");
                
                if (userDto == null)
                    return NotFound();
                
                var userViewModel = new UserViewModel
                {
                    Id = userDto.Id,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Phone = userDto.PhoneNumber ?? string.Empty
                };

                return View(userViewModel);
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
                var success = await _apiService.DeleteAsync($"api/users/{id}?deletedBy=Admin&reason=Silme");
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı silinirken hata oluştu.");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Kullanıcı silinirken hata oluştu.");
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginDto());
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userDto = await _apiService.PostAsync<UserDto>("api/users/login", loginDto);
                    if (userDto != null)
                    {
                        // Session'a kullanıcı bilgilerini kaydet
                        HttpContext.Session.SetString("UserId", userDto.Id.ToString());
                        HttpContext.Session.SetString("UserName", $"{userDto.FirstName} {userDto.LastName}");
                        HttpContext.Session.SetString("UserEmail", userDto.Email);
                        
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Geçersiz email veya şifre.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Giriş yapılırken hata oluştu.");
                }
            }

            return View(loginDto);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
} 