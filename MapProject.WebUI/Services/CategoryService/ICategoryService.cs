using MapProject.DtoLayer.DTOs.CategoryDto;
using Microsoft.AspNetCore.Http;

namespace MapProject.WebUI.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllCategories();
        Task<List<ResultCategoryDto>> GetFavoriteCategories();
        Task<UpdateCategoryDto> GetByIdCategory(string categoryId);

        // 3 resim desteği eklendi (IFormFile? ile opsiyonel yapıldı)
        Task CreateCategory(CreateCategoryDto createCategoryDto, IFormFile? file1, IFormFile? file2, IFormFile? file3);

        // 3 resim desteği eklendi
        Task UpdateCategory(UpdateCategoryDto updateCategoryDto, IFormFile? file1, IFormFile? file2, IFormFile? file3);

        Task DeleteCategory(string categoryId);
    }
}