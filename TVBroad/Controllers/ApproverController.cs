using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TVBroad.Domain.Interfaces.Approver;

namespace TVBroad.Controllers
{
    public class ApproverController : Controller
    {
        private readonly IApproverService _approverService;

        public ApproverController(IApproverService approverService)
        {
            _approverService = approverService;
        }

 

        [HttpGet]
        public async Task<IActionResult> Pending()
        {
            var broadcasts = await _approverService.GetPendingAsync();
            return View(broadcasts);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            await _approverService.ApproveAsync(id);
            return RedirectToAction("Schedule", "Broadcast");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string comment)
        {
            await _approverService.RejectAsync(id, comment);
             return RedirectToAction("Schedule", "Broadcast");
        }
    }
}
