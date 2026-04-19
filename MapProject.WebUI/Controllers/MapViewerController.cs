using MapProject.WebUI.Services.MapViewerService;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.WebUI.Controllers
{
    public class MapViewerController : Controller
    {
        private readonly IMapViewerService _mapViewerService;

        public MapViewerController(IMapViewerService mapViewerService)
        {
            _mapViewerService = mapViewerService;
        }

        // QR ile buraya ID ile gelinecek: /MapViewer/Viewer/123
        [Route("MapViewer/Viewer/{id}")]
        public async Task<IActionResult> Viewer(string id)
        {
            // Servisinizdeki GetByIdMapViewer metodunu çağırıyoruz
            var map = await _mapViewerService.GetByIdMapViewer(id);

            if (map == null) return NotFound();

            // Veriyi view'a gönderiyoruz
            return View(map);
        }
    }
}