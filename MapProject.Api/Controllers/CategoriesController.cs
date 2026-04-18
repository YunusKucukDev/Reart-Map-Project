using MapProject.Api.DTOs.CategoryDto;
using MapProject.Api.Services.CategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MapProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var values = await _service.GetAllCategory();
                return Ok(values);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavoriteCategories()
        {
            var values = await _service.GetFavoriteCategories();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdCategories(string id)
        {
            var values = await _service.GetByIdCategory(id);
            if (values == null) return NotFound();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategories([FromForm] CreateCategoryDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
           
            if (file1 != null) dto.ImageUrl1 = await SaveImage(file1);
            if (file2 != null) dto.ImageUrl2 = await SaveImage(file2);
            if (file3 != null) dto.ImageUrl3 = await SaveImage(file3);

            await _service.CreateCategoryService(dto);
            return Ok(new { message = "Kategori ve Resimler Başarıyla Kaydedildi" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategories([FromForm] UpdateCategoryDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            var existingCategory = await _service.GetByIdCategory(dto.Id);
            if (existingCategory == null) return NotFound("Güncellenecek kategori bulunamadı.");

            // Resim 1 İşlemleri
            if (file1 != null)
            {
                DeleteOldImage(existingCategory.ImageUrl1);
                dto.ImageUrl1 = await SaveImage(file1);
            }
            else
            {
                dto.ImageUrl1 = existingCategory.ImageUrl1;
            }

            // Resim 2 İşlemleri
            if (file2 != null)
            {
                DeleteOldImage(existingCategory.ImageUrl2);
                dto.ImageUrl2 = await SaveImage(file2);
            }
            else
            {
                dto.ImageUrl2 = existingCategory.ImageUrl2;
            }

            // Resim 3 İşlemleri
            if (file3 != null)
            {
                DeleteOldImage(existingCategory.ImageUrl3);
                dto.ImageUrl3 = await SaveImage(file3);
            }
            else
            {
                dto.ImageUrl3 = existingCategory.ImageUrl3;
            }

            await _service.UpdateCategoryDto(dto);
            return Ok("Güncelleme İşlemi Başarılı");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var category = await _service.GetByIdCategory(id);
            if (category != null)
            {
                DeleteOldImage(category.ImageUrl1);
                DeleteOldImage(category.ImageUrl2);
                DeleteOldImage(category.ImageUrl3);
            }

            await _service.DeleteCategoryService(id);
            return Ok("Silme İşlemi Başarılı");
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