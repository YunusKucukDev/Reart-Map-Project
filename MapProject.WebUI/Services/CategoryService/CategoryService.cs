using MapProject.DtoLayer.DTOs.CategoryDto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MapProject.WebUI.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private string BaseUrl = "https://reart-map-project-api.onrender.com";
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateCategory(CreateCategoryDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            using var content = new MultipartFormDataContent();

            // Metin Verileri
            content.Add(new StringContent(dto.CategoryName ?? ""), "CategoryName");
            content.Add(new StringContent(dto.Description ?? ""), "Description");

            // Dosya Kontrolleri
            AddFileToContent(content, file1, "file1");
            AddFileToContent(content, file2, "file2");
            AddFileToContent(content, file3, "file3");

            await _httpClient.PostAsync($"{BaseUrl}/api/Categories", content);
        }

        public async Task UpdateCategory(UpdateCategoryDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            using var content = new MultipartFormDataContent();

            // Metin Verileri
            content.Add(new StringContent(dto.Id), "Id");
            content.Add(new StringContent(dto.CategoryName ?? ""), "CategoryName");
            content.Add(new StringContent(dto.Description ?? ""), "Description");
            content.Add(new StringContent(dto.IsFavorite.ToString()), "IsFavorite");

            // Dosya Kontrolleri
            AddFileToContent(content, file1, "file1");
            AddFileToContent(content, file2, "file2");
            AddFileToContent(content, file3, "file3");

            await _httpClient.PutAsync($"{BaseUrl}/api/Categories", content);
        }

        // Tekrarlanan dosya ekleme işlemini basitleştirmek için yardımcı metod
        private void AddFileToContent(MultipartFormDataContent content, IFormFile? file, string parameterName)
        {
            if (file != null)
            {
                var fileContent = new StreamContent(file.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, parameterName, file.FileName);
            }
        }

        // --- Veri Çekme Metodları ---

        public async Task DeleteCategory(string categoryId)
        {
            await _httpClient.DeleteAsync($"{BaseUrl}/api/Categories/{categoryId}");
        }

        public async Task<List<ResultCategoryDto>> GetAllCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultCategoryDto>>($"{BaseUrl}/api/Categories") ?? new();
        }

        public async Task<List<ResultCategoryDto>> GetFavoriteCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultCategoryDto>>($"{BaseUrl}/api/Categories/favorites") ?? new();
        }

        public async Task<UpdateCategoryDto> GetByIdCategory(string categoryId)
        {
            return await _httpClient.GetFromJsonAsync<UpdateCategoryDto>($"{BaseUrl}/api/Categories/{categoryId}");
        }
    }
}