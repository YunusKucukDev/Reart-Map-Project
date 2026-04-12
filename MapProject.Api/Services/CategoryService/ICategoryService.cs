using MapProject.Api.DTOs.CategoryDto;

namespace MapProject.Api.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllCategory();
        Task<List<ResultCategoryDto>> GetFavoriteCategories();
        Task<GetByIdCategoryId> GetByIdCategory(string id);
        Task UpdateCategoryDto(UpdateCategoryDto dto);
        Task CreateCategoryService(CreateCategoryDto dto);
        Task DeleteCategoryService(string id);
    }
}
