using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.ViewComponents.UILayout
{
    public class FooterUILayout : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
