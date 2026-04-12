using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.ViewComponents.Footer
{
    public class FooterNavbar : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
