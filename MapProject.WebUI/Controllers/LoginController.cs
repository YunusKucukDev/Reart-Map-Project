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
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

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
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Sunucuya bağlanılamadı. Lütfen birkaç saniye bekleyip tekrar deneyin.");
                return View(loginDto);
            }
            catch (TaskCanceledException)
            {
                ModelState.AddModelError(string.Empty, "Sunucu yanıt vermiyor, lütfen tekrar deneyin.");
                return View(loginDto);
            }

            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginDto);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.Name ?? string.Empty),
                new Claim("Token", result.Token ?? string.Empty)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30) });

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
