using MapProject.DtoLayer.DTOs.CoureselDto;
using MapProject.WebUI.Services.CoureselService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminCoureselController : Controller
    {
        private readonly ICoureselService _coureselService;

        public AdminCoureselController(ICoureselService coureselService)
        {
            _coureselService = coureselService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Carousel Slaytlar";
            ViewData["ActiveMenu"] = "Couresel";
            var list = await _coureselService.GetAllCouresels();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Yeni Slayt";
            ViewData["ActiveMenu"] = "Couresel";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCoureselDto dto, IFormFile? file1)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Yeni Slayt";
                ViewData["ActiveMenu"] = "Couresel";
                return View(dto);
            }

            await _coureselService.CreateCouresel(dto, file1);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Title"] = "Slayt Düzenle";
            ViewData["ActiveMenu"] = "Couresel";
            var item = await _coureselService.GetByIdCouresel(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCoureselDto dto, IFormFile? file1)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Slayt Düzenle";
                ViewData["ActiveMenu"] = "Couresel";
                return View(dto);
            }

            await _coureselService.UpdateCouresel(dto, file1);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Admin/AdminCouresel/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));
            await _coureselService.DeleteCouresel(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
