using MapProject.Api.DTOs.VisitorLogDto;

namespace MapProject.Api.Services.VisitorLogService
{
    public interface IVisitorLogService
    {
        Task CreateAsync(CreateVisitorLogDto dto);
        Task<long> GetTotalCountAsync();
        Task<List<ResultVisitorLogDto>> GetRecentAsync(int count);
        Task<List<DailyVisitorCountDto>> GetDailyCountsAsync(int days);
    }

    public class DailyVisitorCountDto
    {
        public string Date { get; set; }   // "2025-04-12"
        public int Count { get; set; }
    }
}
