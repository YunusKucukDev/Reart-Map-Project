using MapProject.Api.DTOs.VisitorLogDto;
using MapProject.Api.Services.VisitorLogService;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorLogsController : ControllerBase
    {
        private readonly IVisitorLogService _service;

        public VisitorLogsController(IVisitorLogService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVisitorLogDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            var count = await _service.GetTotalCountAsync();
            return Ok(count);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int count = 10)
        {
            var list = await _service.GetRecentAsync(count);
            return Ok(list);
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDaily([FromQuery] int days = 7)
        {
            var list = await _service.GetDailyCountsAsync(days);
            return Ok(list);
        }
    }
}
