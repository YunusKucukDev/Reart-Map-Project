using MapProject.Api.DTOs.UserInformationDto;

namespace MapProject.Api.Services.UserInformationService
{
    public interface IUserInformationService
    {
        Task<ResultUserInformationDto> GetUserInformation();
        Task UpdateUserInformationDto(UpdateUserInformationDto dto);
        Task CreateUserInformationDto(CreateUserInformationDto dto);
        Task DeleteUserInformationDto(string id);
    }
}
