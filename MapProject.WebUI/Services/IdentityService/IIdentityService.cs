using MapProject.DtoLayer.DTOs.LoginDto;
using MapProject.DtoLayer.DTOs.UserIdentityDto;

namespace MapProject.WebUI.Services.IdentityService
{
    public interface IIdentityService
    {
        Task<UserIdentityDto?> Login(LoginDto loginDto);
    }
}
