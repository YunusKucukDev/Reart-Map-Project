using MapProject.DtoLayer.DTOs.ContactDto;

namespace MapProject.WebUI.Services.ContactService
{
    public class ContactService : IContactService
    {
        private readonly HttpClient _httpClient;

        public ContactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateContactService(CreateContactDto dto)
        {
            await _httpClient.PostAsJsonAsync("api/Contacts", dto);
        }

        public async Task DeleteContactService(string id)
        {
            await _httpClient.DeleteAsync($"api/Contacts/{id}");
        }

        public async Task<List<ResultContactDto>> GetAllContact()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultContactDto>>("api/Contacts");
        }

        public async Task<GetByIdContactDto> GetByIdContact(string id)
        {
            return await _httpClient.GetFromJsonAsync<GetByIdContactDto>($"api/Contacts/{id}");
        }

        public async Task UpdateContactDto(UpdateContactDto dto)
        {
            await _httpClient.PutAsJsonAsync("api/Contacts", dto);
        }
    }
}
