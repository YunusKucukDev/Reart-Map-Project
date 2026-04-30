using MapProject.WebUI.Hubs;
using MapProject.WebUI.Services.MapViewerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MapProject.WebUI.Controllers
{
    public class MapViewerController : Controller
    {
        private readonly IMapViewerService _mapViewerService;
        private readonly IHubContext<VisitorHub> _hubContext;
        private readonly ILogger<MapViewerController> _logger;

        public MapViewerController(IMapViewerService mapViewerService, IHubContext<VisitorHub> hubContext, ILogger<MapViewerController> logger)
        {
            _mapViewerService = mapViewerService;
            _hubContext = hubContext;
            _logger = logger;
        }

        [Route("MapViewer/Viewer/{id}")]
        public async Task<IActionResult> Viewer(string id)
        {
            var map = await _mapViewerService.GetByIdMapViewer(id);
            if (map == null) return NotFound();
            return View(map);
        }

        [HttpPost]
        [Route("MapViewer/Track/{id}")]
        public async Task<IActionResult> Track(string id)
        {
            int newCount = await _mapViewerService.IncrementViewCountAsync(id);

            // SUNUCU LOGU: VS Output penceresine bakýn
            _logger.LogInformation(">>> Sinyal Gönderiliyor: ID={Id}, YeniSayi={Count}", id, newCount);

            await _hubContext.Clients.Group("admins").SendAsync("UpdateMapViewerCount", id, newCount);
            return Ok(newCount);
        }
    }
}
