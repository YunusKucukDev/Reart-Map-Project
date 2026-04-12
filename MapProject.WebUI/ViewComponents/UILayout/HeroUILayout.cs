using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.ViewComponents.UILayout
{
    public class HeroUILayout : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
