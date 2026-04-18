using MapProject.DtoLayer.DTOs.LoginDto;
using MapProject.DtoLayer.DTOs.UserIdentityDto;
using MapProject.WebUI.Services.IdentityService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MapProject.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IIdentityService _identityService;

        public LoginController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Eğer zaten giriş yapmışsa Admin paneline gönder
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "AdminDashboard", new { area = "Admin" });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto loginDto)
        {
            if (!ModelState.IsValid)

                return View(loginDto);

            UserIdentityDto? result;
            try
            {
                result = await _identityService.Login(loginDto);
            }
            catch (Exception ex)
            {
                // Bağlantı hatası durumunda detaylı bilgi veriyoruz
                ModelState.AddModelError(string.Empty, $"Bağlantı Hatası: {ex.Message}");
                return View(loginDto);
            }

            // API'den null dönüyorsa (Kullanıcı adı/şifre yanlış veya URL hatalı)
            if (result == null || string.IsNullOrEmpty(result.Token))
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı ya da sunucu yetki vermedi.");
                return View(loginDto);
            }

            // Başarılı giriş: Claim'leri oluştur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.Name ?? result.Name ?? "User"),
                new Claim("Token", result.Token)
            };

            // Eğer API'den Roller geliyorsa onları da ekle (Yetki hatalarını önlemek için)
            // if (result.Roles != null) { foreach(var role in result.Roles) claims.Add(new Claim(ClaimTypes.Role, role)); }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            // Başarılı giriş sonrası Admin Dashboard'a yönlendir
            return RedirectToAction("Index", "AdminDashboard", new { area = "Admin" });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}