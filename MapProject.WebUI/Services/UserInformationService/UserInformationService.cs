using MapProject.DtoLayer.DTOs.UserInformationDto;
using System.Net.Http.Json;

namespace MapProject.WebUI.Services.UserInformationService
{
    public class UserInformationService : IUserInformationService
    {
        private string BaseUrl = "https://localhost:5000";
        private readonly HttpClient _httpClient;

        public UserInformationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateUserInformationDto(CreateUserInformationDto dto, IFormFile? file)
        {
            using var content = new MultipartFormDataContent();

            foreach (var prop in dto.GetType().GetProperties())
            {
                var value = prop.GetValue(dto)?.ToString() ?? "";
                if (prop.Name != "Image") content.Add(new StringContent(value), prop.Name);
            }

            if (file != null)
            {
                var fileContent = new StreamContent(file.OpenReadStream());
                content.Add(fileContent, "file", file.FileName);
            }

            await _httpClient.PostAsync($"{BaseUrl}/api/UserInformations", content);
        }

        public async Task UpdateUserInformationDto(UpdateUserInformationDto dto, IFormFile? file)
        {
            using var content = new MultipartFormDataContent();

            // DTO içindeki TÜM özellikleri otomatik olarak pakete ekle (Surname, Email, Linkler dahil)
            foreach (var prop in dto.GetType().GetProperties())
            {
                var value = prop.GetValue(dto)?.ToString() ?? "";
                if (prop.Name != "Image") // Image zaten dosya olarak veya manuel eklenecek
                {
                    content.Add(new StringContent(value), prop.Name);
                }
            }

            if (file != null)
            {
                var fileContent = new StreamContent(file.OpenReadStream());
                // ÖNEMLİ: API'deki parametre adı "file" olduğu için burası "file" kalmalı
                content.Add(fileContent, "file", file.FileName);
            }

            var response = await _httpClient.PutAsync($"{BaseUrl}/api/UserInformations", content);
            response.EnsureSuccessStatusCode(); // Hata varsa burada patlar, hatayı görürsün
        }

        public async Task<ResultUserInformationDto> GetUserInformation()
        {
            return await _httpClient.GetFromJsonAsync<ResultUserInformationDto>($"{BaseUrl}/api/UserInformations");
        }

        public async Task<UpdateUserInformationDto> GetByIdUserInformation(string id)
        {
            return await _httpClient.GetFromJsonAsync<UpdateUserInformationDto>($"{BaseUrl}/api/UserInformations/{id}");
        }

        public async Task DeleteUserInformationDto(string id)
        {
            await _httpClient.DeleteAsync($"{BaseUrl}/api/UserInformations/{id}");
        }
    }
}