using MapProject.DtoLayer.DTOs.ContactDto;

namespace MapProject.WebUI.Services.ContactService
{
    public class ContactService : IContactService
    {

        private string BaseUrl = "https://localhost:5000";
        private readonly HttpClient _httpClient;

        public ContactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateContactService(CreateContactDto dto)
        {
            await _httpClient.PostAsJsonAsync(BaseUrl + "/api/Contacts", dto);
        }

        public async Task DeleteContactService(string id)
        {
            await _httpClient.DeleteAsync(BaseUrl + "/api/Contacts/" + id);
        }

        public async Task<List<ResultContactDto>> GetAllContact()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultContactDto>>(BaseUrl + "/api/Contacts");
        }

        public async Task<GetByIdContactDto> GetByIdContact(string id)
        {

            return await _httpClient.GetFromJsonAsync<GetByIdContactDto>(BaseUrl + "/api/Contacts/" + id);
        }

        public async Task UpdateContactDto(UpdateContactDto dto)
        {
            await _httpClient.PutAsJsonAsync(BaseUrl + "/api/Contacts", dto);
        }
    }
}
