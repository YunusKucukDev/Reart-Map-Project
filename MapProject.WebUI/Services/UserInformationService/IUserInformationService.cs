using MapProject.DtoLayer.DTOs.UserInformationDto;
using Microsoft.AspNetCore.Http; // IFormFile için eklendi

namespace MapProject.WebUI.Services.UserInformationService
{
    public interface IUserInformationService
    {
        Task<ResultUserInformationDto> GetUserInformation();

        // Resim dosyası parametre olarak eklendi (IFormFile? ile opsiyonel yapıldı)
        Task UpdateUserInformationDto(UpdateUserInformationDto dto, IFormFile? file);

        // Resim dosyası parametre olarak eklendi
        Task CreateUserInformationDto(CreateUserInformationDto dto, IFormFile? file);

        Task DeleteUserInformationDto(string id);

        // Güncelleme yaparken eski resmi korumak için ID ile veri getirme metodu
        Task<UpdateUserInformationDto> GetByIdUserInformation(string id);
    }
}