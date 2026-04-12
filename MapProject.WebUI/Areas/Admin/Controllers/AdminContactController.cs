using MapProject.WebUI.Services.ContactService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminContactController : Controller
    {
        private readonly IContactService _contactService;

        public AdminContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Mesajlar";
            ViewData["ActiveMenu"] = "Contact";
            var contacts = await _contactService.GetAllContact();
            return View(contacts);
        }

        public async Task<IActionResult> Detail(string id)
        {
            ViewData["Title"] = "Mesaj Detayı";
            ViewData["ActiveMenu"] = "Contact";
            var contact = await _contactService.GetByIdContact(id);
            if (contact == null) return NotFound();
            return View(contact);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _contactService.DeleteContactService(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
