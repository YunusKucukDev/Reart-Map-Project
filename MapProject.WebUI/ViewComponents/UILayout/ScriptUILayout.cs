using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.ViewComponents.UILayout
{
    public class ScriptUILayout : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
