using MapProject.DtoLayer.DTOs.CategoryDto;
using MapProject.WebUI.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public AdminCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Kategoriler";
            ViewData["ActiveMenu"] = "Category";
            var categories = await _categoryService.GetAllCategories();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Yeni Kategori";
            ViewData["ActiveMenu"] = "Category";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Yeni Kategori";
                ViewData["ActiveMenu"] = "Category";
                return View(dto);
            }

            await _categoryService.CreateCategory(dto, file1, file2, file3);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Title"] = "Kategori Düzenle";
            ViewData["ActiveMenu"] = "Category";
            var category = await _categoryService.GetByIdCategory(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryDto dto, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Kategori Düzenle";
                ViewData["ActiveMenu"] = "Category";
                return View(dto);
            }

            // Interface 3 parametre beklediği için file1, file2 ve file3'ü gönderiyoruz
            await _categoryService.UpdateCategory(dto, file1, file2, file3);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Admin/AdminCategory/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));

            await _categoryService.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ToggleFavorite(string id)
        {
            var category = await _categoryService.GetByIdCategory(id);
            if (category == null) return NotFound();

            var dto = new UpdateCategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description,
                ImageUrl1 = category.ImageUrl1,
                ImageUrl2 = category.ImageUrl2,
                ImageUrl3 = category.ImageUrl3,
                IsFavorite = !category.IsFavorite
            };

            // Favori durumunda yeni resim yüklenmediği için tüm dosya parametrelerini null geçiyoruz
            await _categoryService.UpdateCategory(dto, null, null, null);

            return RedirectToAction(nameof(Index));
        }
    }
}