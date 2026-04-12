using MapProject.DtoLayer.DTOs.MapIdentityDescriptionDto;
using MapProject.WebUI.Services.MapIdentityDescriptionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminMapIdentityDescriptionController : Controller
    {
        private readonly IMapIdentityDescriptionService _service;

        public AdminMapIdentityDescriptionController(IMapIdentityDescriptionService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Harita Tanımı";
            ViewData["ActiveMenu"] = "MapIdentity";
            var data = await _service.GetMapIdentityDescription();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateMapIdentityDescriptionDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen formdaki eksiklikleri kontrol edin.";
                return RedirectToAction(nameof(Index)); // Hata durumunda Index'e geri dönmek en güvenlisidir
            }

            try
            {
                await _service.UpdateMapIdentityDescriptionDto(dto, file1, file2, file3);
                TempData["Success"] = "Harita tanımı başarıyla güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Güncelleme sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMapIdentityDescriptionDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            // Servis katmanına dosyaları da gönderiyoruz
            await _service.CreateMapIdentityDescriptionDto(dto, file1, file2, file3);

            TempData["Success"] = "Harita tanımı oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }
    }
}
