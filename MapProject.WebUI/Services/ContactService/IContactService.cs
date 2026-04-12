using MapProject.DtoLayer.DTOs.ContactDto;

namespace MapProject.WebUI.Services.ContactService
{
    public interface IContactService
    {
        Task<List<ResultContactDto>> GetAllContact();
        Task<GetByIdContactDto> GetByIdContact(string id);
        Task UpdateContactDto(UpdateContactDto dto);
        Task CreateContactService(CreateContactDto dto);
        Task DeleteContactService(string id);
    }
}
