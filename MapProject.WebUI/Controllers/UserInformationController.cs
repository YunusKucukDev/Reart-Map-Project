using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.Controllers
{
    public class UserInformationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
