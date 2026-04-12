using MapProject.DtoLayer.DTOs.ContactDto;
using MapProject.WebUI.Services.ContactService;
using MapProject.WebUI.Services.EmailService;
using MapProject.WebUI.Services.UserInformationService;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IUserInformationService _userInformationService;
        private readonly IEmailService _emailService;
        private readonly ILogger<ContactController> _logger;

        public ContactController(
            IContactService contactService,
            IUserInformationService userInformationService,
            IEmailService emailService,
            ILogger<ContactController> logger)
        {
            _contactService = contactService;
            _userInformationService = userInformationService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(new CreateContactDto());
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateContactDto createContactDto)
        {
            // 1. MongoDB'ye kaydet
            await _contactService.CreateContactService(createContactDto);

            // 2. E-posta gönder (hata olursa kaydı engellemez)
            try
            {
                await _emailService.SendContactEmailAsync(
                    createContactDto.UserName,
                    createContactDto.UserEmail,
                    createContactDto.UserSubject ?? "Konu belirtilmedi",
                    createContactDto.Message);

                TempData["ContactSuccess"] = "Mesajınız iletildi, en kısa sürede dönüş yapılacaktır.";
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "E-posta gönderilemedi, mesaj yine de kaydedildi.");
                TempData["ContactSuccess"] = "Mesajınız kaydedildi, en kısa sürede dönüş yapılacaktır.";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
