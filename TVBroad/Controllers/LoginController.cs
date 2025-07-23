using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TVBroad.Domain.Interfaces.IUser;
using TVBroad.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace TVBroad.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        // Show login page
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

       

            // Authentication with cookie // Handle login
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Username and password are required.";
                return View();
            }

            var user = await _userService.ValidateUserAsync(username, password);

            if (user == null)
            {
                ViewBag.Error = "Invalid credentials.";
                return View();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Redirect based on role
            //return user.Role switch
            //{
            //    "Admin" => RedirectToAction("Index", "Admin"),
            //    "Scheduler" => RedirectToAction("In", "Scheduler"),
            //    "Approver" => RedirectToAction("Schedule", "Broadcast"),
            //    _ => RedirectToAction("Schedule")
            //};
            return RedirectToAction("Schedule","Broadcast");
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
