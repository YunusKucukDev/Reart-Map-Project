using MapProject.DtoLayer.DTOs.VideoDto;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace MapProject.WebUI.Services.VideoService
{
    public class VideoService : IVideoService
    {
        private readonly HttpClient _httpClient;

        public VideoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultVideoDto>> GetAllVideos()
        {
            return await SafeGetJsonList<ResultVideoDto>("api/Videos");
        }

        public async Task<ResultVideoDto?> GetFeaturedVideo()
        {
            return await SafeGetJson<ResultVideoDto>("api/Videos/featured");
        }

        public async Task<UpdateVideoDto?> GetByIdVideo(string id)
        {
            return await SafeGetJson<UpdateVideoDto>($"api/Videos/{id}");
        }

        public async Task CreateVideo(CreateVideoDto dto, IFormFile? videoFile, IFormFile? thumbnailFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(dto.Title ?? ""), "Title");
            content.Add(new StringContent(dto.Description ?? ""), "Description");
            content.Add(new StringContent(dto.Tag ?? ""), "Tag");
            content.Add(new StringContent(dto.Duration ?? ""), "Duration");
            content.Add(new StringContent(dto.IsFeatured.ToString()), "IsFeatured");
            content.Add(new StringContent(dto.Order.ToString()), "Order");

            if (videoFile != null)
                content.Add(await ToStreamContent(videoFile), "videoFile", videoFile.FileName);

            if (thumbnailFile != null)
                content.Add(await ToStreamContent(thumbnailFile), "thumbnailFile", thumbnailFile.FileName);

            await _httpClient.PostAsync("api/Videos", content);
        }

        public async Task UpdateVideo(UpdateVideoDto dto, IFormFile? videoFile, IFormFile? thumbnailFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(dto.Id), "Id");
            content.Add(new StringContent(dto.Title ?? ""), "Title");
            content.Add(new StringContent(dto.Description ?? ""), "Description");
            content.Add(new StringContent(dto.VideoUrl ?? ""), "VideoUrl");
            content.Add(new StringContent(dto.ThumbnailUrl ?? ""), "ThumbnailUrl");
            content.Add(new StringContent(dto.Tag ?? ""), "Tag");
            content.Add(new StringContent(dto.Duration ?? ""), "Duration");
            content.Add(new StringContent(dto.IsFeatured.ToString()), "IsFeatured");
            content.Add(new StringContent(dto.Order.ToString()), "Order");

            if (videoFile != null)
                content.Add(await ToStreamContent(videoFile), "videoFile", videoFile.FileName);

            if (thumbnailFile != null)
                content.Add(await ToStreamContent(thumbnailFile), "thumbnailFile", thumbnailFile.FileName);

            await _httpClient.PutAsync("api/Videos", content);
        }

        public async Task DeleteVideo(string id)
        {
            await _httpClient.DeleteAsync($"api/Videos/{id}");
        }

        public async Task SetFeatured(string id)
        {
            await _httpClient.PutAsync($"api/Videos/{id}/set-featured", null);
        }

        private static async Task<StreamContent> ToStreamContent(IFormFile file)
        {
            var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;
            var sc = new StreamContent(ms);
            sc.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            return sc;
        }

        private static readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

        private async Task<T?> SafeGetJson<T>(string url) where T : class
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;
                var body = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(body) || body.Trim() == "null") return null;
                return JsonSerializer.Deserialize<T>(body, _jsonOpts);
            }
            catch { return null; }
        }

        private async Task<List<T>> SafeGetJsonList<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return new();
                var body = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(body) || body.Trim() == "null") return new();
                return JsonSerializer.Deserialize<List<T>>(body, _jsonOpts) ?? new();
            }
            catch { return new(); }
        }
    }
}
