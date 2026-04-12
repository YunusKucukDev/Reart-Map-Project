using MapProject.Api.DTOs.ContactDto;
using MapProject.Api.Services.ContactService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _service;

        public ContactsController(IContactService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContact()
        {
            var value = await _service.GetAllContact();
            return Ok(value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdContact(string id)
        {
            var value = await _service.GetByIdContact(id);
            return Ok(value);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContact(UpdateContactDto dto)
        {
            await _service.UpdateContactDto(dto);
            return Ok("Başarılı");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteContact(string id)
        {
            await _service.DeleteContactService(id);
            return Ok("Silme İşlemi Başarılı");
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(CreateContactDto dto)
        {
            await _service.CreateContactService(dto);
            return Ok("Başarılı");
        }
    }
}
