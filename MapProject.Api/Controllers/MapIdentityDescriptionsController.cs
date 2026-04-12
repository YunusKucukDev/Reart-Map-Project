using MapProject.Api.DTOs.MapIdentityDescriptionDto;
using MapProject.Api.Services.MapIdentityDescriptionService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MapProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapIdentityDescriptionsController : ControllerBase
    {
        private readonly IMapIdentityDescriptionService _service;
        private string GetWebUIPath() => Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "MapProject.WebUI", "wwwroot", "images");

        public MapIdentityDescriptionsController(IMapIdentityDescriptionService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var value = await _service.GetByIdMapIdentityDescription(id);
            return Ok(value);
        }

        [HttpGet]
        public async Task<IActionResult> GetMapIdentityDescription()
        {
            try
            {
                var value = await _service.GetMapIdentityDescription();
                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.GetType().Name, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMapIdentitiyDescription([FromForm] CreateMapIdentityDescriptionDto dto,
            IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            // Kod tekrarını önlemek için SaveFile metodunu kullanıyoruz
            dto.ImageUrl1 = await SaveFile(file1);
            dto.ImageUrl2 = await SaveFile(file2);
            dto.ImageUrl3 = await SaveFile(file3);

            await _service.CreateMapIdentityDescriptionDto(dto);
            return Ok("Ekleme İşlemi Başarılı");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMapIdentityDescription([FromForm] UpdateMapIdentityDescriptionDto dto,
            IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            if (string.IsNullOrEmpty(dto.Id)) return BadRequest("ID bulunamadı.");

            var existing = await _service.GetByIdMapIdentityDescription(dto.Id);
            if (existing == null) return NotFound("Kayıt bulunamadı.");

            dto.ImageUrl1 = file1 != null ? await SaveFile(file1) : (dto.ImageUrl1 ?? existing.ImageUrl1);
            dto.ImageUrl2 = file2 != null ? await SaveFile(file2) : (dto.ImageUrl2 ?? existing.ImageUrl2);
            dto.ImageUrl3 = file3 != null ? await SaveFile(file3) : (dto.ImageUrl3 ?? existing.ImageUrl3);

            await _service.UpdateMapIdentityDescriptionDto(dto);
            return Ok("Güncelleme başarılı");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMapIdentityDescription(string id)
        {
            // İstersen burada eski dosyaları fiziksel olarak silme mantığı da ekleyebilirsin.
            await _service.DeleteMapIdentityDescriptionDto(id);
            return Ok("Silme İşlemi Başarılı");
        }

        [NonAction]
        private async Task<string?> SaveFile(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            // Hedef: WebUI projesindeki wwwroot/images
            var targetPath = GetWebUIPath();

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(targetPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/images/" + fileName;
        }
    }
}
