using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.ViewComponents.UILayout
{
    public class NavbarUILayout : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
