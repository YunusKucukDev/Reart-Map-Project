using MapProject.DtoLayer.DTOs.VideoDto;
using MapProject.WebUI.Services.VideoService;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.ViewComponents.Home
{
    public class HomeVideoSectionViewModel
    {
        public ResultVideoDto? Featured { get; set; }
        public List<ResultVideoDto> Videos { get; set; } = new();
    }

    public class HomeVideoSection : ViewComponent
    {
        private readonly IVideoService _videoService;

        public HomeVideoSection(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var featured = await _videoService.GetFeaturedVideo();
                var all = await _videoService.GetAllVideos();

                var model = new HomeVideoSectionViewModel
                {
                    Featured = featured,
                    Videos = all.Where(v => !v.IsFeatured).OrderBy(v => v.Order).ToList()
                };

                return View(model);
            }
            catch
            {
                return View(new HomeVideoSectionViewModel());
            }
        }
    }
}
