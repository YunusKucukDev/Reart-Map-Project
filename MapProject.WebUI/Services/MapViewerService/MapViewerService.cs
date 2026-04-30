using MapProject.DtoLayer.DTOs.MapViewerDto;
using System.Net.Http.Headers;

namespace MapProject.WebUI.Services.MapViewerService
{
    public class MapViewerService : IMapViewerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MapViewerService> _logger;

        public MapViewerService(HttpClient httpClient, ILogger<MapViewerService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<ResultMapViewerDto>> GetAllMapViewer()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultMapViewerDto>>("api/MapViewers") ?? new();
        }

        public async Task<GetByIdMapViewerDto> GetByIdMapViewer(string id)
        {
            return await _httpClient.GetFromJsonAsync<GetByIdMapViewerDto>($"api/MapViewers/{id}");
        }

        public async Task CreateMapViewer(CreateMapViewerDto dto, IFormFile? file1)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(dto.Title ?? ""), "Title");

            if (file1 != null)
            {
                var fileContent = new StreamContent(file1.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file1.ContentType);
                content.Add(fileContent, "file1", file1.FileName);
            }

            await _httpClient.PostAsync("api/MapViewers", content);
        }

        public async Task UpdateMapViewer(UpdateMapViewerDto dto, IFormFile? file1)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(dto.Id), "Id");
            content.Add(new StringContent(dto.Title ?? ""), "Title");
            content.Add(new StringContent(dto.ImageUrl ?? ""), "ImageUrl");

            if (file1 != null)
            {
                var fileContent = new StreamContent(file1.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file1.ContentType);
                content.Add(fileContent, "file1", file1.FileName);
            }

            await _httpClient.PutAsync("api/MapViewers", content);
        }

        public async Task DeleteMapViewer(string id)
        {
            await _httpClient.DeleteAsync($"api/MapViewers?id={id}");
        }

        public async Task<int> IncrementViewCountAsync(string id)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/MapViewers/{id}/view", null);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<int>();

                var body = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("IncrementViewCount failed. Status: {Status}, Body: {Body}, URL: {Url}",
                    response.StatusCode, body, _httpClient.BaseAddress + $"api/MapViewers/{id}/view");
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "IncrementViewCount exception for id {Id}", id);
                return 0;
            }
        }
    }
}
