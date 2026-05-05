using Microsoft.AspNetCore.Mvc;
using FestivalWasteTracker.Models;
using System.Linq;

namespace FestivalWasteTracker.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("User", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                // Everyone goes to dashboard
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Invalid Username or Password";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            var user = new User
            {
                Username = username,
                Password = password,
                Role = "viewer"
            }; 

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}