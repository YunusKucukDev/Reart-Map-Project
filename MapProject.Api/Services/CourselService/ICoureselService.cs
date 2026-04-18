using MapProject.Api.DTOs.CoureselDto;

namespace MapProject.Api.Services.CourselService
{
    public interface ICoureselService
    {
        Task<List<ResultCoureselDto>> GetAllCouresel();
        Task<ResultCoureselDto> GetByIdCouresel(string id);
        Task UpdateCoureselDto(UpdateCoureselDto dto);
        Task CreateCoureselService(CreateCoureselDto dto);
        Task DeleteCoureselService(string id);
    }
}
