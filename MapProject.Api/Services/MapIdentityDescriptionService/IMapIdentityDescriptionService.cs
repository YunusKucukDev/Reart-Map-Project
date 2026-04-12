using MapProject.Api.DTOs.MapIdentityDescriptionDto;

namespace MapProject.Api.Services.MapIdentityDescriptionService
{
    public interface IMapIdentityDescriptionService
    {
        Task<ResultMapIdentityDescriptionDto> GetMapIdentityDescription();
        Task<UpdateMapIdentityDescriptionDto> GetByIdMapIdentityDescription(string id); // Eksik olan eklendi
        Task UpdateMapIdentityDescriptionDto(UpdateMapIdentityDescriptionDto dto);
        Task CreateMapIdentityDescriptionDto(CreateMapIdentityDescriptionDto dto);
        Task DeleteMapIdentityDescriptionDto(string id);
    }
}