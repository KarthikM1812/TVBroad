using Microsoft.AspNetCore.Mvc;
using TVBroad.Domain.Interfaces;

public class BroadcastController : Controller
{
    private readonly IBroadcastService _broadcastService;

    public BroadcastController(IBroadcastService broadcastService)
    {
        _broadcastService = broadcastService;
    }

    // ✅ Common view for all roles: shows only approved broadcasts
    [HttpGet]
    public async Task<IActionResult> Schedule()
    {
        var broadcasts = await _broadcastService.GetApprovedBroadcastsAsync();
        return View(broadcasts);
    }

    public IActionResult ScheduleJson()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Json", "Broadcast.json");
        var json = System.IO.File.ReadAllText(filePath);
        return Content(json, "application/json");
    }
}
