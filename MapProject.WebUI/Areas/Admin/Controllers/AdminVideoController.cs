using MapProject.DtoLayer.DTOs.VideoDto;
using MapProject.WebUI.Services.VideoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminVideoController : Controller
    {
        private readonly IVideoService _videoService;

        public AdminVideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Videolar";
            ViewData["ActiveMenu"] = "Video";
            try
            {
                var featured = await _videoService.GetFeaturedVideo();
                var all = await _videoService.GetAllVideos();
                ViewBag.Featured = featured;
                return View(all);
            }
            catch
            {
                ViewBag.Featured = null;
                TempData["Error"] = "API'ye bağlanılamadı. Lütfen API sunucusunun çalıştığını kontrol edin.";
                return View(new List<MapProject.DtoLayer.DTOs.VideoDto.ResultVideoDto>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Yeni Video";
            ViewData["ActiveMenu"] = "Video";
            return View();
        }

        [HttpPost]
        [RequestSizeLimit(500_000_000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 500_000_000)]
        public async Task<IActionResult> Create(CreateVideoDto dto, IFormFile? videoFile, IFormFile? thumbnailFile)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Yeni Video";
                ViewData["ActiveMenu"] = "Video";
                return View(dto);
            }
            dto.IsFeatured = false;
            await _videoService.CreateVideo(dto, videoFile, thumbnailFile);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CreateFeatured()
        {
            ViewData["Title"] = "Öne Çıkan Video";
            ViewData["ActiveMenu"] = "Video";
            return View();
        }

        [HttpPost]
        [RequestSizeLimit(500_000_000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 500_000_000)]
        public async Task<IActionResult> CreateFeatured(CreateVideoDto dto, IFormFile? videoFile, IFormFile? thumbnailFile)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Öne Çıkan Video";
                ViewData["ActiveMenu"] = "Video";
                return View(dto);
            }
            dto.IsFeatured = true;
            await _videoService.CreateVideo(dto, videoFile, thumbnailFile);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Title"] = "Video Düzenle";
            ViewData["ActiveMenu"] = "Video";
            var item = await _videoService.GetByIdVideo(id);
            if (item is null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [RequestSizeLimit(500_000_000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 500_000_000)]
        public async Task<IActionResult> Edit(UpdateVideoDto dto, IFormFile? videoFile, IFormFile? thumbnailFile)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Video Düzenle";
                ViewData["ActiveMenu"] = "Video";
                return View(dto);
            }
            await _videoService.UpdateVideo(dto, videoFile, thumbnailFile);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Admin/AdminVideo/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));
            await _videoService.DeleteVideo(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Admin/AdminVideo/SetFeatured/{id}")]
        public async Task<IActionResult> SetFeatured(string id)
        {
            await _videoService.SetFeatured(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
