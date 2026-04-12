using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MapProject.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
