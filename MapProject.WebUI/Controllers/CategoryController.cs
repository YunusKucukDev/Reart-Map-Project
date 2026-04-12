using MapProject.WebUI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MapProject.WebUI.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategories();
            return View(categories);
        }

        public async Task<IActionResult> Details(string id)
        {
            var category = await _categoryService.GetByIdCategory(id);
            if (category == null)
                return NotFound();
            return View(category);
        }
    }
}
