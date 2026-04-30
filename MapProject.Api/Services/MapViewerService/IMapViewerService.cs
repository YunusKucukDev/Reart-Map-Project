using MapProject.Api.DTOs.MapViewerDto;

namespace MapProject.Api.Services.MapViewerService
{
    public interface IMapViewerService
    {
        Task<List<ResultMapViewerDto>> GetAllMapViewers();
        Task<GetByIdMapViewerDto> GetByIsMapViewer(string id);
        Task UpdateMapViewer(UpdateMapViewerDto dto);
        Task DeleteMapViewer(string id);
        Task CreateMapViewer(CreateMapViewerDto dto);
        Task<int> IncrementViewCountAsync(string id);
    }
}
