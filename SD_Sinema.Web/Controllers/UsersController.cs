using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD_Sinema.Web.Models;
using SD_Sinema.Web.Models.DTOs;
using System.Text;

namespace SD_Sinema.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/users");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var userDtos = JsonConvert.DeserializeObject<List<UserDto>>(content);
                    
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
            }
            catch
            {
                // API bağlantı hatası
            }

            return View(Enumerable.Empty<UserViewModel>());
        }

        public IActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
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

                    var json = JsonConvert.SerializeObject(createUserDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/users", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
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
                var response = await _httpClient.GetAsync($"api/users/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var userDto = JsonConvert.DeserializeObject<UserDto>(content);
                    
                    if (userDto == null)
                        return NotFound();
                    
                    var userViewModel = new UserViewModel
                    {
                        Id = userDto.Id,
                        FirstName = userDto.FirstName,
                        LastName = userDto.LastName,
                        Email = userDto.Email,
                        Phone = userDto.PhoneNumber ?? string.Empty,
                        CreatedAt = userDto.CreatedDate
                    };
                    
                    return View(userViewModel);
                }
            }
            catch
            {
                // API bağlantı hatası
            }

            return NotFound();
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
                        Id = id,
                        FirstName = userViewModel.FirstName,
                        LastName = userViewModel.LastName,
                        Email = userViewModel.Email,
                        Phone = userViewModel.Phone
                    };

                    var json = JsonConvert.SerializeObject(updateUserDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"api/users/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
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
                var response = await _httpClient.DeleteAsync($"api/users/{id}?deletedBy=Admin&reason=Silme");
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

        public IActionResult Login()
        {
            return View(new LoginDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(loginDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/users/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<UserDto>(responseContent);
                        
                        if (user == null)
                        {
                            ModelState.AddModelError("", "Giriş yapılırken hata oluştu.");
                            return View(loginDto);
                        }
                        
                        // Session'a kullanıcı bilgilerini kaydet
                        HttpContext.Session.SetString("UserId", user.Id.ToString());
                        HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
                        HttpContext.Session.SetString("UserRole", user.Role);

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