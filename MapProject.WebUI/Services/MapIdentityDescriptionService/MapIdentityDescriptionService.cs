using MapProject.DtoLayer.DTOs.MapIdentityDescriptionDto;
using System.Net.Http.Json;
using System.Text.Json;

namespace MapProject.WebUI.Services.MapIdentityDescriptionService
{
    public class MapIdentityDescriptionService : IMapIdentityDescriptionService
    {
        private readonly HttpClient _httpClient;

        public MapIdentityDescriptionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<ResultMapIdentityDescriptionDto> GetMapIdentityDescription()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/MapIdentityDescriptions");
                var body = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new ResultMapIdentityDescriptionDto();
                }
                return JsonSerializer.Deserialize<ResultMapIdentityDescriptionDto>(body, _jsonOptions);
            }
            catch (Exception ex)
            {
                // Hatayı burada loglayabilirsin
                return new ResultMapIdentityDescriptionDto();
            }
        }

        public async Task CreateMapIdentityDescriptionDto(CreateMapIdentityDescriptionDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            using var content = new MultipartFormDataContent();
            PrepareMultipartContent(content, dto);
            AddFileToContent(content, file1, "file1");
            AddFileToContent(content, file2, "file2");
            AddFileToContent(content, file3, "file3");

            await _httpClient.PostAsync("api/MapIdentityDescriptions", content);
        }

        public async Task UpdateMapIdentityDescriptionDto(UpdateMapIdentityDescriptionDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            using var content = new MultipartFormDataContent();
            PrepareMultipartContent(content, dto);
            AddFileToContent(content, file1, "file1");
            AddFileToContent(content, file2, "file2");
            AddFileToContent(content, file3, "file3");

            var response = await _httpClient.PutAsync("api/MapIdentityDescriptions", content);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Hatası ({(int)response.StatusCode}): {body}");
            }
        }

        private void PrepareMultipartContent(MultipartFormDataContent content, object dto)
        {
            foreach (var prop in dto.GetType().GetProperties())
            {
                var value = prop.GetValue(dto);
                if (value != null) // ImageUrl kontrolünü buradan kaldırın!
                {
                    var stringContent = new StringContent(value.ToString() ?? "");
                    // Tırnak işaretlerini kaldırarak denemenizi öneririm
                    content.Add(stringContent, prop.Name);
                }
            }
        }

        private void AddFileToContent(MultipartFormDataContent content, IFormFile? file, string parameterName)
        {
            if (file != null)
            {
                var fileContent = new StreamContent(file.OpenReadStream());
                // Content-Type eklemek önemlidir
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                // "parameterName" burada file1, file2 veya file3 olacaktır.
                content.Add(fileContent, parameterName, file.FileName);
            }
        }

        public async Task<UpdateMapIdentityDescriptionDto> GetByIdMapIdentityDescription(string id)
        {
            return await _httpClient.GetFromJsonAsync<UpdateMapIdentityDescriptionDto>($"api/MapIdentityDescriptions/{id}");
        }

        public async Task DeleteMapIdentityDescriptionDto(string id)
        {
            await _httpClient.DeleteAsync($"api/MapIdentityDescriptions/{id}");
        }
    }
}