using MapProject.WebUI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MapProject.WebUI.ViewComponents.Home
{
    public class HomeCategoryList : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public HomeCategoryList(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllCategories();
            return View(categories);
        }
    }
}
