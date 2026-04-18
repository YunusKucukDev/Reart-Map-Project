using MapProject.Api.DTOs.CoureselDto;
using MapProject.Api.Services.CourselService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoureselsControllers : ControllerBase
    {
        private readonly ICoureselService _service;

        public CoureselsControllers(ICoureselService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCouresel()
        {
            var res = await _service.GetAllCouresel();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdCouresel(string id)
        {
            var res = await _service.GetByIdCouresel(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCouresel([FromForm] CreateCoureselDto dto, IFormFile? file1)
        {
            if (file1 != null) dto.ImageUrl = await SaveImage(file1);
           

            await _service.CreateCoureselService(dto);
            return Ok(new { message = "couresl ve resim Başarıyla Kaydedildi" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCouresel([FromForm] UpdateCoureselDto dto, IFormFile? file1)
        {
            var existingCouresel = await _service.GetByIdCouresel(dto.Id);
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

            await _service.UpdateCoureselDto(dto);
            return Ok("Güncelleme İşlemi Başarılı");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCouresel(string id)
        {
            await _service.DeleteCoureselService(id);
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
