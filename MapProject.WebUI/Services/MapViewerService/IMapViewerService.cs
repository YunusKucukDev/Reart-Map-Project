using MapProject.DtoLayer.DTOs.MapViewerDto;

namespace MapProject.WebUI.Services.MapViewerService
{
    public interface IMapViewerService
    {
        Task<List<ResultMapViewerDto>> GetAllMapViewer();
        Task<GetByIdMapViewerDto> GetByIdMapViewer(string id);
        Task UpdateMapViewer(UpdateMapViewerDto dto, IFormFile? file1);
        Task DeleteMapViewer(string id);
        Task CreateMapViewer(CreateMapViewerDto dto, IFormFile? file1);
        Task<int> IncrementViewCountAsync(string id);
    }
}
