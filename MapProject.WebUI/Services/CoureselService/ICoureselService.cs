using MapProject.DtoLayer.DTOs.CoureselDto;

namespace MapProject.WebUI.Services.CoureselService
{
    public interface ICoureselService
    {
        Task<List<ResultCoureselDto>> GetAllCouresels();
        Task<UpdateCoureselDto> GetByIdCouresel(string id);
        Task CreateCouresel(CreateCoureselDto dto, IFormFile? file1);
        Task UpdateCouresel(UpdateCoureselDto dto, IFormFile? file1);
        Task DeleteCouresel(string id);
    }
}
