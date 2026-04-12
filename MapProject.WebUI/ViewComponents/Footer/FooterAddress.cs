using MapProject.WebUI.Services.UserInformationService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MapProject.WebUI.ViewComponents.Footer
{
    public class FooterAddress : ViewComponent
    {

        private readonly IUserInformationService _userInformationService;

        public FooterAddress(IUserInformationService userInformationService)
        {
            _userInformationService = userInformationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userInformation = await _userInformationService.GetUserInformation();
            return View(userInformation);
        }
    }
}
