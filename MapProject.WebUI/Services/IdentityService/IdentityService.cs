using MapProject.DtoLayer.DTOs.LoginDto;
using MapProject.DtoLayer.DTOs.UserIdentityDto;

namespace MapProject.WebUI.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public IdentityService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<UserIdentityDto?> Login(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{_config["ApiSettings:BaseUrl"]}/api/Accounts/login", loginDto);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<UserIdentityDto>();
        }
    }
}
