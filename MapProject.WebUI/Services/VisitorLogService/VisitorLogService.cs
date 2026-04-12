using MapProject.DtoLayer.DTOs.VisitorLogDto;
using System.Net.Http.Json;

namespace MapProject.WebUI.Services.VisitorLogService
{
    public class VisitorLogService : IVisitorLogService
    {
        private const string BaseUrl = "https://localhost:5000";
        private readonly HttpClient _httpClient;

        public VisitorLogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(CreateVisitorLogDto dto)
        {
            await _httpClient.PostAsJsonAsync($"{BaseUrl}/api/VisitorLogs", dto);
        }

        public async Task<long> GetTotalCountAsync()
        {
            return await _httpClient.GetFromJsonAsync<long>($"{BaseUrl}/api/VisitorLogs/total");
        }

        public async Task<List<ResultVisitorLogDto>> GetRecentAsync(int count = 10)
        {
            return await _httpClient.GetFromJsonAsync<List<ResultVisitorLogDto>>(
                $"{BaseUrl}/api/VisitorLogs/recent?count={count}") ?? new();
        }

        public async Task<List<DailyVisitorCountDto>> GetDailyCountsAsync(int days = 7)
        {
            return await _httpClient.GetFromJsonAsync<List<DailyVisitorCountDto>>(
                $"{BaseUrl}/api/VisitorLogs/daily?days={days}") ?? new();
        }
    }
}
