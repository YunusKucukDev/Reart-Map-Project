using MapProject.DtoLayer.DTOs.LoginDto;
using MapProject.DtoLayer.DTOs.UserIdentityDto;
using System.Net.Http.Json;

namespace MapProject.WebUI.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;

        // IConfiguration'a artýk burada ihtiyacýmýz yok, Program.cs'de kullandýk.
        public IdentityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserIdentityDto?> Login(LoginDto loginDto)
        {
           
            var response = await _httpClient.PostAsJsonAsync("api/Accounts/login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
               
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UserIdentityDto>();
        }
    }
}