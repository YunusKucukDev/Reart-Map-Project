using MapProject.DtoLayer.DTOs.VisitorLogDto;
using MapProject.WebUI.Services.VisitorLogService;
using MapProject.WebUI.Services.VisitorService;
using Microsoft.AspNetCore.SignalR;

namespace MapProject.WebUI.Hubs
{
    public class VisitorHub : Hub
    {
        private readonly VisitorTracker _tracker;
        private readonly IVisitorLogService _logService;

        public VisitorHub(VisitorTracker tracker, IVisitorLogService logService)
        {
            _tracker = tracker;
            _logService = logService;
        }

        // Public site tarafından çağrılır
        public async Task RegisterVisitor()
        {
            _tracker.Add(Context.ConnectionId);

            var ip = Context.GetHttpContext()?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var ua = Context.GetHttpContext()?.Request.Headers["User-Agent"].ToString() ?? "unknown";

            // Veritabanına kaydet (fire-and-forget; hata olursa sistemi bloklamaz)
            _ = Task.Run(async () =>
            {
                try
                {
                    await _logService.CreateAsync(new CreateVisitorLogDto
                    {
                        VisitedAt = DateTime.UtcNow,
                        IpAddress = ip,
                        UserAgent = ua
                    });
                }
                catch { /* loglama eklenebilir */ }
            });

            await Clients.Group("admins").SendAsync("UpdateVisitorCount", _tracker.Count);
        }

        // Admin panel tarafından çağrılır
        public async Task RegisterAdmin()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "admins");
            await Clients.Caller.SendAsync("UpdateVisitorCount", _tracker.Count);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            bool wasVisitor = _tracker.Remove(Context.ConnectionId);
            if (wasVisitor)
            {
                await Clients.Group("admins").SendAsync("UpdateVisitorCount", _tracker.Count);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
