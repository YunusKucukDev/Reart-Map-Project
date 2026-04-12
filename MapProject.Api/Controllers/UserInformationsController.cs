using MapProject.Api.DTOs.UserInformationDto;
using MapProject.Api.Services.UserInformationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInformationsController : ControllerBase
    {
        private readonly IUserInformationService _service;

        public UserInformationsController(IUserInformationService service)
        {
            _service = service;
        }

        // WebUI projesinin fiziksel yolunu döndüren yardımcı metot
        private string GetWebUIPath() => Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "MapProject.WebUI", "wwwroot");

        [HttpGet]
        public async Task<IActionResult> GetUserInformation()
        {
            var value = await _service.GetUserInformation();
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserInformation([FromForm] CreateUserInformationDto dto, IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                // Doğrudan SaveImage metodunu kullanalım (kod tekrarı olmasın)
                dto.Image = await SaveImage(file);
            }

            await _service.CreateUserInformationDto(dto);
            return Ok("Kullanıcı Bilgisi ve Görsel Başarıyla Eklendi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserInformation([FromForm] UpdateUserInformationDto dto, IFormFile? file)
        {
            var existingUser = await _service.GetUserInformation();
            if (existingUser == null) return NotFound("Kullanıcı bilgisi bulunamadı");

            if (file != null && file.Length > 0)
            {
                // Eski resmi WebUI klasöründen siler
                DeleteOldImage(existingUser.Image);

                // Yeni resmi WebUI klasörüne kaydeder
                dto.Image = await SaveImage(file);
            }
            else
            {
                dto.Image = existingUser.Image;
            }

            await _service.UpdateUserInformationDto(dto);
            return Ok("Güncelleme İşlemi Başarılı");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserInformation(string id)
        {
            // İsteğe bağlı: Silmeden önce resmi de silebilirsin
            var existing = await _service.GetUserInformation();
            if (existing != null) DeleteOldImage(existing.Image);

            await _service.DeleteUserInformationDto(id);
            return Ok("Silme İşlemi Başarılı");
        }

        [NonAction]
        private async Task<string> SaveImage(IFormFile file)
        {
            // WebUI projesindeki images klasörünü hedefle
            var imagesPath = Path.Combine(GetWebUIPath(), "images");

            if (!Directory.Exists(imagesPath)) Directory.CreateDirectory(imagesPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(imagesPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/images/" + fileName;
        }

        [NonAction]
        private void DeleteOldImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl) || !imageUrl.StartsWith("/images/")) return;
            try
            {
                // WebUI projesinin içinden dosyayı bul ve sil
                var oldFilePath = Path.Combine(GetWebUIPath(), imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);
            }
            catch { }
        }
    }
}