using MapProject.Api.DTOs.ContactDto;

namespace MapProject.Api.Services.ContactService
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
