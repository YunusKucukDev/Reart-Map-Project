using MapProject.WebUI.Services.UserInformationService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MapProject.WebUI.ViewComponents.Footer
{
    public class FooterSocialLinks : ViewComponent
    {
        private readonly IUserInformationService _userInformationService;

        public FooterSocialLinks(IUserInformationService userInformationService)
        {
            _userInformationService = userInformationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _userInformationService.GetUserInformation();
            return View(result);
        }
    }
}
