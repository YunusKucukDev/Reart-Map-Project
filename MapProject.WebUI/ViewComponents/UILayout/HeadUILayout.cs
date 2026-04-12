using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.ViewComponents.UILayout
{
    public class HeadUILayout : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
