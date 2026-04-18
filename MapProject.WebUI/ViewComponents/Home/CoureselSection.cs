using MapProject.WebUI.Services.CoureselService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MapProject.WebUI.ViewComponents.Home
{
    public class CoureselSection : ViewComponent
    {

        private readonly ICoureselService _service;

        public CoureselSection(ICoureselService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var value = await _service.GetAllCouresels();
            return View(value);
        }
    }
}
