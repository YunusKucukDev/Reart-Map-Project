using MapProject.WebUI.Services.ContactService;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.ViewComponents.Home
{
    public class HomeContact : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
