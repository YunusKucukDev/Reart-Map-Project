using MapProject.Api.DTOs.MapViewerDto;
using MapProject.Api.Services.MapViewerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapViewersController : ControllerBase
    {
        
        private readonly IMapViewerService _service;

        public MapViewersController(IMapViewerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMapViewers()
        {
            var res = await _service.GetAllMapViewers();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdMapViewers(string id)
        {
            var res = await _service.GetByIsMapViewer(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMapViewers([FromForm] CreateMapViewerDto dto, IFormFile? file1)
        {
            if (file1 != null) dto.ImageUrl = await SaveImage(file1);


            await _service.CreateMapViewer(dto);
            return Ok(new { message = "couresl ve resim Başarıyla Kaydedildi" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMapViewers([FromForm] UpdateMapViewerDto dto, IFormFile? file1)
        {
            var existingCouresel = await _service.GetByIsMapViewer(dto.Id);
            if (existingCouresel == null) return NotFound("Güncellenecek kategori bulunamadı.");

            // Resim 1 İşlemleri
            if (file1 != null)
            {
                DeleteOldImage(existingCouresel.ImageUrl);
                dto.ImageUrl = await SaveImage(file1);
            }
            else
            {
                dto.ImageUrl = existingCouresel.ImageUrl;
            }

            await _service.UpdateMapViewer(dto);
            return Ok("Güncelleme İşlemi Başarılı");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMapViewers(string id)
        {
            await _service.DeleteMapViewer(id);
            return Ok("silindi");
        }

        // --- YARDIMCI METOTLAR (Dosya Yönetimi) ---

        private async Task<string> SaveImage(IFormFile file)
        {
            // API'nin kendi wwwroot/images klasörünü hedefle
            var apiPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(apiPath))
                Directory.CreateDirectory(apiPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(apiPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Veritabanına tam URL veya sadece API yolu olarak kaydedilir
            // Örn: https://api.ajansreart.com/images/dosya.jpg
            return "/images/" + fileName;
        }

        private void DeleteOldImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            try
            {
                // Resim yolu "/images/" ile başlıyorsa API'nin kendi wwwroot klasörüne bak
                if (imageUrl.StartsWith("/images/"))
                {
                    // API'nin çalıştığı kök dizindeki wwwroot klasörünü bulur
                    var apiWwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                    // Veritabanındaki "/images/dosya.jpg" yolunu fiziksel yola çevirir
                    // Path.Combine ("/images/..." kısmındaki baştaki slash'ı otomatik yönetir ama garantiye almak için TrimStart iyidir)
                    var oldFilePath = Path.Combine(apiWwwrootPath, imageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Canlı ortamda hatayı takip etmek istersen buraya log ekleyebilirsin
                //_logger.LogError($"Resim silinirken hata: {ex.Message}");
            }
        }


    }
}
