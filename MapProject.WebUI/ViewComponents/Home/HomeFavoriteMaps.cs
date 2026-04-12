using MapProject.WebUI.Services.CategoryService;

using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.ViewComponents.Home
{
    public class HomeFavoriteMaps : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public HomeFavoriteMaps(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetFavoriteCategories();
            return View(categories);
        }
    }
}
