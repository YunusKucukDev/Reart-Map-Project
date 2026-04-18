using MapProject.DtoLayer.DTOs.CoureselDto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MapProject.WebUI.Services.CoureselService
{
    public class CoureselService : ICoureselService
    {
        private readonly HttpClient _httpClient;

        public CoureselService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultCoureselDto>> GetAllCouresels()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultCoureselDto>>("api/CoureselsControllers") ?? new();
        }

        public async Task<UpdateCoureselDto> GetByIdCouresel(string id)
        {
            return await _httpClient.GetFromJsonAsync<UpdateCoureselDto>($"api/CoureselsControllers/{id}");
        }

        public async Task CreateCouresel(CreateCoureselDto dto, IFormFile? file1)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(dto.Title ?? ""), "Title");
            content.Add(new StringContent(dto.Descripton ?? ""), "Descripton");

            if (file1 != null)
            {
                var fileContent = new StreamContent(file1.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file1.ContentType);
                content.Add(fileContent, "file1", file1.FileName);
            }

            await _httpClient.PostAsync("api/CoureselsControllers", content);
        }

        public async Task UpdateCouresel(UpdateCoureselDto dto, IFormFile? file1)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(dto.Id), "Id");
            content.Add(new StringContent(dto.Title ?? ""), "Title");
            content.Add(new StringContent(dto.Descripton ?? ""), "Descripton");
            content.Add(new StringContent(dto.ImageUrl ?? ""), "ImageUrl");

            if (file1 != null)
            {
                var fileContent = new StreamContent(file1.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file1.ContentType);
                content.Add(fileContent, "file1", file1.FileName);
            }

            await _httpClient.PutAsync("api/CoureselsControllers", content);
        }

        public async Task DeleteCouresel(string id)
        {
            await _httpClient.DeleteAsync($"api/CoureselsControllers?id={id}");
        }
    }
}
