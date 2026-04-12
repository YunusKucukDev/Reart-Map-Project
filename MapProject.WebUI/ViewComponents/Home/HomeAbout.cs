using MapProject.WebUI.Services.UserInformationService;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.ViewComponents.Home
{
    public class HomeAbout : ViewComponent
    {
        private readonly IUserInformationService _userInformationService;

        public HomeAbout(IUserInformationService userInformationService)
        {
            _userInformationService = userInformationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userInfo = await _userInformationService.GetUserInformation();
            return View(userInfo);
        }
    }
}
