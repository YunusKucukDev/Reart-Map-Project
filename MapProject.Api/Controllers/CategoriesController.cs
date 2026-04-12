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
            // Resimler varsa kaydet ve URL'lerini DTO'ya ata
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
            // API projesinden bir üst klasöre çık ve WebUI projesinin images klasörünü hedefle
            // Proje adının "MapProject.WebUI" olduğundan emin ol, değilse klasör adını düzelt.
            var webUIPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "MapProject.WebUI", "wwwroot", "images");

            if (!Directory.Exists(webUIPath))
                Directory.CreateDirectory(webUIPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(webUIPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/images/" + fileName; // Veritabanına yine aynı formatta kaydediyoruz
        }

        private void DeleteOldImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            try
            {
                if (imageUrl.StartsWith("/images/"))
                {
                    // Silme işlemi için de aynı WebUI yoluna gidiyoruz
                    var webUIPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "MapProject.WebUI", "wwwroot");
                    var oldFilePath = Path.Combine(webUIPath, imageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }
            catch { /* Loglanabilir */ }
        }
    }
}