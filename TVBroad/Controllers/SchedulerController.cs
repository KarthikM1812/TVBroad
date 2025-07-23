using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TVBroad.Domain.Entities;
using TVBroad.Domain.Interfaces.IScheduler;

namespace TVBroad.Controllers
{
    public class SchedulerController : Controller
    {
        private readonly ISchedulerService _schedulerService;

        public SchedulerController(ISchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
        }

        public async Task<IActionResult> Index()
        {
            var broadcasts = await _schedulerService.GetAllAsync();
            return RedirectToAction("Schedule", "Broadcast");
        }

        // GET: Create Schedule Form
        public IActionResult Create()
        {
            return View();
        }


        // POST: Broadcasts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Scheduler")]
        public async Task<IActionResult> Create(BroadcastSchedule schedule)
        {

            schedule.Status = "Pending";
            schedule.CreatedBy = User.Identity?.Name ?? "Unknown";

            //bool hasOverlap = await _schedulerService.HasOverlapAsync(broadcast.StartTime, broadcast.EndTime);
            //if (hasOverlap)
            //{
            //    ModelState.AddModelError("", "Broadcast time overlaps with an existing approved broadcast.");
            //    return View(broadcast);
            //}S

            await _schedulerService.AddAsync(schedule);
            return RedirectToAction("Schedule", "Broadcast");
        }
        
         // GET: Edit
    public async Task<IActionResult> Edit(int id)
        {
            var schedule = await _schedulerService.GetByIdAsync(id);
            if (schedule == null)
                return NotFound();

            return View(schedule);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BroadcastSchedule schedule)
        {
            //if (id != schedule.Id)
            //    return NotFound();

            //if (!ModelState.IsValid)
            //{
            //    // Check errors
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
            //    // Log or inspect this for debugging
            //    var original = await _schedulerService.GetByIdAsync(id);
            //    if (original != null)
            //        schedule.CreatedBy = original.CreatedBy;
            //    return View(schedule);
            //}

            var existing = await _schedulerService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            //  Update only editable fields
            existing.Title = schedule.Title;
            existing.Description = schedule.Description;
            existing.StartTime = schedule.StartTime;
            existing.EndTime = schedule.EndTime;

            await _schedulerService.UpdateAsync(existing);
            return RedirectToAction("Schedule", "Broadcast");
        }


        // GET: Delete Confirm
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _schedulerService.GetByIdAsync(id);
            return View(schedule);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _schedulerService.DeleteAsync(id);
            return RedirectToAction("Schedule", "Broadcast");
        }
    }
}
