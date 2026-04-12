using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.ViewComponents.Home
{
    public class HomeWorkProcess : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
