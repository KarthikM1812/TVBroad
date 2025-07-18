using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TVBroad.Domain.Entities;
using TVBroad.Domain.Interfaces;
using TVBroad.Domain.Interfaces.Broadcast;

namespace Broadcast.Controllers
{
    public class BroadcastController : Controller
    {
        private readonly IBroadcastService _broadcastService;

        public BroadcastController(IBroadcastService broadcastService)
        {
            _broadcastService = broadcastService;
        }
        public IActionResult ScheduleJson()

        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Json", "Broadcast.json");

            var json = System.IO.File.ReadAllText(filePath);

            return Content(json, "application/json");

        }


        // List all broadcasts (Admin or Approver view)
        public async Task<IActionResult> Index()
        {
            var broadcasts = await _broadcastService.GetAllAsync();
            return View(broadcasts);
        }

        // GET: Create
        [Authorize(Roles = "Scheduler")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Scheduler")]
        //public async Task<IActionResult> Create(Broadcasting schedule)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _broadcastService.CreateBroadcastAsync(schedule);
        //        return RedirectToAction("Index"); // ? Redirect to Index after save
        //    }
        //    return View(schedule); // if validation fails, show same view
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Scheduler")]
        public async Task<IActionResult> Create(Broadcasting schedule)
        {
            if (ModelState.IsValid)
            {
                schedule.Status = "Pending";
                //schedule.CreatedAt = User.Identity.Name; 
                await _broadcastService.CreateBroadcastAsync(schedule);
                return RedirectToAction("Index");
            }
            return View(schedule);
        }


        // GET: Edit
        [Authorize(Roles = "Scheduler")]
        public async Task<IActionResult> Edit(int id)
        {
            var broadcast = await _broadcastService.GetByIdAsync(id);
            if (broadcast == null || broadcast.Status == "Approved")
                return NotFound();

            return View(broadcast);
        }

        // POST: Edit
        [HttpPost]
        [Authorize(Roles = "Scheduler")]
        public async Task<IActionResult> Edit(Broadcasting schedule)
        {
            if (!ModelState.IsValid)
                return View(schedule);

            if (await _broadcastService.IsOverlappingAsync(schedule.StartTime, schedule.EndTime, schedule.Id))
            {
                ModelState.AddModelError("", "The schedule overlaps with an existing one.");
                return View(schedule);
            }

            schedule.Status = "Pending"; // Re-submit for approval
            await _broadcastService.UpdateBroadcastAsync(schedule);
            return RedirectToAction("Index");
        }

        // GET: Delete
        [Authorize(Roles = "Scheduler")]
        public async Task<IActionResult> Delete(int id)
        {
            var broadcast = await _broadcastService.GetByIdAsync(id);
            if (broadcast == null)
                return NotFound();

            return View(broadcast);
        }

        // POST: Delete
        [Authorize(Roles = "Scheduler")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _broadcastService.DeleteBroadcastAsync( id); 
            return RedirectToAction("Index");
        }

        // GET: Approver View
        [Authorize(Roles = "Approver")]
        public async Task<IActionResult> ApproveList()
        {
            var broadcasts = await _broadcastService.GetAllAsync();
            var pending = broadcasts.FindAll(x => x.Status == "Pending");
            return View(pending);
        }

        // GET: Approve or Reject
        [Authorize(Roles = "Approver")]
        public async Task<IActionResult> Review(int id)
        {
            var broadcast = await _broadcastService.GetByIdAsync(id);
            if (broadcast == null)
                return NotFound();

            return View(broadcast);
        }

        // POST: Review
        [HttpPost]
        [Authorize(Roles = "Approver")]
        public async Task<IActionResult> Review(int id, string decision)
        {
            var broadcast = await _broadcastService.GetByIdAsync(id);
            if (broadcast == null)
                return NotFound();

            broadcast.Status = decision == "Approve" ? "Approved" : "Rejected";
            await _broadcastService.UpdateBroadcastAsync(broadcast);
            return RedirectToAction("ApproveList");
        }
    }
}
