using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.Controllers
{
    public class UIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
