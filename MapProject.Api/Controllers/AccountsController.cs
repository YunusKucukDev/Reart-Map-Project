using MapProject.Api.DTOs.LoginDto;
using MapProject.Api.DTOs.UserIdentityDto;
using MapProject.Api.Entities;
using MapProject.Api.Services.IdentityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;

        public AccountsController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserIdentityDto>> Login(LoginDto model)
        {
            // 1. Kullanıcıyı bul
            var user = await _userManager.FindByNameAsync(model.UserName);

            // 2. Kullanıcı var mı ve şifre doğru mu kontrol et
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new ProblemDetails { Title = "Kullanıcı adı veya şifre hatalı" });
            }

            // 3. Token oluştur ve kullanıcı bilgilerini dön
            return Ok(new UserIdentityDto
            {
                Token = await _tokenService.GenerateToken(user),
                Name = user.Name
            });
        }


        [Authorize]
        [HttpGet("getuser")]
        public async Task<ActionResult<UserIdentityDto>> GetUser()
        {
            // Token'daki Name Claim'i üzerinden kullanıcıyı bulur
            var user = await _userManager.FindByNameAsync(User.Identity?.Name!);
            if (user == null) return NotFound();

            return Ok(new UserIdentityDto
            {
                Token = await _tokenService.GenerateToken(user),
                Name = user.Name
            });
        }

    }
}
