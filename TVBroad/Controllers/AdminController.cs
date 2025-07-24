using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TVBroad.Domain.Entities;
using TVBroad.Domain.Interfaces.IAdmin;
using System.Collections.Generic;

namespace TVBroad.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminUserService _adminService;

        public AdminController(IAdminUserService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            //var users = await _adminService.GetAllUsersAsync();
            return RedirectToAction("Schedule", "Broadcast");
        }

        public async Task<IActionResult> Role()
        {
            var users = await _adminService.GetAllUsersAsync(); 
            return View(users);
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(string username, string password, string role)
        {
            var user = new AppUser
            {
                Username = username,
                PasswordHash = password, 
                Role = role
            };

            await _adminService.AddUserAsync(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(int id, string role)
        {
            await _adminService.AssignRoleAsync(id, role);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(int id)
        {
            await _adminService.RemoveRoleAsync(id);
            return RedirectToAction("Index");
        }
    }
}
