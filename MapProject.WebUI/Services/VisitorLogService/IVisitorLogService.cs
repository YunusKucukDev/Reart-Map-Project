using MapProject.DtoLayer.DTOs.VisitorLogDto;

namespace MapProject.WebUI.Services.VisitorLogService
{
    public interface IVisitorLogService
    {
        Task CreateAsync(CreateVisitorLogDto dto);
        Task<long> GetTotalCountAsync();
        Task<List<ResultVisitorLogDto>> GetRecentAsync(int count = 10);
        Task<List<DailyVisitorCountDto>> GetDailyCountsAsync(int days = 7);
    }
}
