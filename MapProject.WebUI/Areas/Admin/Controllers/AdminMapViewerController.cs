using MapProject.DtoLayer.DTOs.MapViewerDto;
using MapProject.WebUI.Services.MapViewerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminMapViewerController : Controller
    {
        private readonly IMapViewerService _mapViewerService;

        public AdminMapViewerController(IMapViewerService mapViewerService)
        {
            _mapViewerService = mapViewerService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Harita Görüntüleyiciler";
            ViewData["ActiveMenu"] = "MapViewer";
            var list = await _mapViewerService.GetAllMapViewer();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Yeni Harita Ekle";
            ViewData["ActiveMenu"] = "MapViewer";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMapViewerDto dto, IFormFile? file1)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Yeni Harita Ekle";
                ViewData["ActiveMenu"] = "MapViewer";
                return View(dto);
            }

            await _mapViewerService.CreateMapViewer(dto, file1);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Title"] = "Harita Düzenle";
            ViewData["ActiveMenu"] = "MapViewer";
            var item = await _mapViewerService.GetByIdMapViewer(id);
            if (item == null) return NotFound();

            var dto = new UpdateMapViewerDto
            {
                Id = item.Id,
                Title = item.Title,
                ImageUrl = item.ImageUrl,
               
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateMapViewerDto dto, IFormFile? file1)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Harita Düzenle";
                ViewData["ActiveMenu"] = "MapViewer";
                return View(dto);
            }

            await _mapViewerService.UpdateMapViewer(dto, file1);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Admin/AdminMapViewer/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));
            await _mapViewerService.DeleteMapViewer(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
