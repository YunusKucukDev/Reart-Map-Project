using MapProject.WebUI.Services.UserInformationService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MapProject.WebUI.ViewComponents.Contact
{
    public class ContactAbout : ViewComponent
    {
        private readonly IUserInformationService _userInformationService;

        public ContactAbout(IUserInformationService userInformationService)
        {
            _userInformationService = userInformationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values  = await _userInformationService.GetUserInformation();
            return View(values);
        }   
    }
}
