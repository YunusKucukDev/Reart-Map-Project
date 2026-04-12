using MapProject.WebUI.Services.MapIdentityDescriptionService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MapProject.WebUI.Controllers
{
    public class MapIdentityDescriptionController : Controller
    {

        private readonly IMapIdentityDescriptionService _mapIdentityDescriptionService;

        public MapIdentityDescriptionController(IMapIdentityDescriptionService mapIdentityDescriptionService)
        {
            _mapIdentityDescriptionService = mapIdentityDescriptionService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _mapIdentityDescriptionService.GetMapIdentityDescription();
            return View(result);
        }
    }
}
