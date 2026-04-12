using MapProject.DtoLayer.DTOs.UserInformationDto;
using MapProject.WebUI.Services.UserInformationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminUserInformationController : Controller
    {
        private readonly IUserInformationService _userInformationService;

        public AdminUserInformationController(IUserInformationService userInformationService)
        {
            _userInformationService = userInformationService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Firma Bilgileri";
            ViewData["ActiveMenu"] = "UserInformation";
            var info = await _userInformationService.GetUserInformation();
            return View(info);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserInformationDto dto, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Firma Bilgileri";
                ViewData["ActiveMenu"] = "UserInformation";
                return View("Index", dto);
            }
            await _userInformationService.UpdateUserInformationDto(dto, file);
            TempData["Success"] = "Firma bilgileri güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserInformationDto dto, IFormFile? file)
        {
            await _userInformationService.CreateUserInformationDto(dto, file);
            TempData["Success"] = "Firma bilgileri oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }
    }
}
