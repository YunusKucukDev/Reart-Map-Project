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
            // Program.cs'de tanýmlanan BaseAddress sayesinde sadece endpoint yolunu yazýyoruz.
            // API'deki Controller ismine göre "api/Account/login" veya "api/Accounts/login" 
            // olarak kontrol etmeyi unutma!
            var response = await _httpClient.PostAsJsonAsync("api/Accounts/login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                // Hatanýn ne olduðunu loglamak istersen:
                // var error = await response.Content.ReadAsStringAsync();
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UserIdentityDto>();
        }
    }
}