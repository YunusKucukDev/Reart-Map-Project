using MapProject.DtoLayer.DTOs.MapIdentityDescriptionDto;
using Microsoft.AspNetCore.Http;

namespace MapProject.WebUI.Services.MapIdentityDescriptionService
{
    public interface IMapIdentityDescriptionService
    {
        Task<ResultMapIdentityDescriptionDto> GetMapIdentityDescription();
        Task UpdateMapIdentityDescriptionDto(UpdateMapIdentityDescriptionDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3);
        Task CreateMapIdentityDescriptionDto(CreateMapIdentityDescriptionDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3);
        Task DeleteMapIdentityDescriptionDto(string id);
        Task<UpdateMapIdentityDescriptionDto> GetByIdMapIdentityDescription(string id);
    }
}