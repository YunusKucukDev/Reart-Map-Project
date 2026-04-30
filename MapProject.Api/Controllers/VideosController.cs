using MapProject.Api.DTOs.VideoDto;
using MapProject.Api.Services.VideoService;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _service;

        public VideosController(IVideoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllVideos();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdVideo(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeatured()
        {
            var result = await _service.GetFeaturedVideo();
            return Ok(result);
        }

        [HttpPost]
        [RequestSizeLimit(500_000_000)] // 500 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 500_000_000)]
        public async Task<IActionResult> Create([FromForm] CreateVideoDto dto,
                                                IFormFile? videoFile,
                                                IFormFile? thumbnailFile)
        {
            if (videoFile != null)
                dto.VideoUrl = await SaveFile(videoFile, "videos");

            if (thumbnailFile != null)
                dto.ThumbnailUrl = await SaveFile(thumbnailFile, "images");

            await _service.CreateVideo(dto);
            return Ok(new { message = "Video başarıyla oluşturuldu." });
        }

        [HttpPut]
        [RequestSizeLimit(500_000_000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 500_000_000)]
        public async Task<IActionResult> Update([FromForm] UpdateVideoDto dto,
                                                IFormFile? videoFile,
                                                IFormFile? thumbnailFile)
        {
            var existing = await _service.GetByIdVideo(dto.Id);
            if (existing is null) return NotFound("Güncellenecek video bulunamadı.");

            if (videoFile != null)
            {
                DeleteOldFile(existing.VideoUrl);
                dto.VideoUrl = await SaveFile(videoFile, "videos");
            }
            else
            {
                dto.VideoUrl = existing.VideoUrl;
            }

            if (thumbnailFile != null)
            {
                DeleteOldFile(existing.ThumbnailUrl);
                dto.ThumbnailUrl = await SaveFile(thumbnailFile, "images");
            }
            else
            {
                dto.ThumbnailUrl = existing.ThumbnailUrl;
            }

            await _service.UpdateVideo(dto);
            return Ok("Güncelleme başarılı.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _service.GetByIdVideo(id);
            if (existing != null)
            {
                DeleteOldFile(existing.VideoUrl);
                DeleteOldFile(existing.ThumbnailUrl);
            }

            await _service.DeleteVideo(id);
            return Ok("Video silindi.");
        }

        [HttpPut("{id}/set-featured")]
        public async Task<IActionResult> SetFeatured(string id)
        {
            var existing = await _service.GetByIdVideo(id);
            if (existing is null) return NotFound();

            await _service.SetFeatured(id);
            return Ok("Öne çıkan video güncellendi.");
        }

        private async Task<string> SaveFile(IFormFile file, string folder)
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(dir, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{folder}/{fileName}";
        }

        private void DeleteOldFile(string? url)
        {
            if (string.IsNullOrEmpty(url)) return;
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", url.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            catch { }
        }
    }
}
