using MapProject.WebUI.Services.CategoryService;
using MapProject.WebUI.Services.ContactService;
using MapProject.WebUI.Services.VisitorLogService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MapProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminDashboardController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IContactService _contactService;
        private readonly IVisitorLogService _visitorLogService;

        public AdminDashboardController(
            ICategoryService categoryService,
            IContactService contactService,
            IVisitorLogService visitorLogService)
        {
            _categoryService = categoryService;
            _contactService = contactService;
            _visitorLogService = visitorLogService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Dashboard";
            ViewData["ActiveMenu"] = "Dashboard";

            var categories = await _categoryService.GetAllCategories();
            var contacts   = await _contactService.GetAllContact();
            var totalVisitors = await _visitorLogService.GetTotalCountAsync();
            var recentVisitors = await _visitorLogService.GetRecentAsync(8);
            var dailyCounts    = await _visitorLogService.GetDailyCountsAsync(7);

            ViewBag.CategoryCount   = categories?.Count ?? 0;
            ViewBag.ContactCount    = contacts?.Count ?? 0;
            ViewBag.TotalVisitors   = totalVisitors;
            ViewBag.RecentVisitors  = recentVisitors;
            ViewBag.DailyLabels     = JsonSerializer.Serialize(dailyCounts.Select(x => x.Date).ToList());
            ViewBag.DailyData       = JsonSerializer.Serialize(dailyCounts.Select(x => x.Count).ToList());

            return View();
        }
    }
}
