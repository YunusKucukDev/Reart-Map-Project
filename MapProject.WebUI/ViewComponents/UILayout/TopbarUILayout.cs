using MapProject.WebUI.Services.UserInformationService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MapProject.WebUI.ViewComponents.UILayout
{
    public class TopbarUILayout : ViewComponent
    {

        private readonly IUserInformationService _userInformationService;

        public TopbarUILayout(IUserInformationService userInformationService)
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
