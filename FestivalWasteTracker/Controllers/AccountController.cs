using FestivalWasteTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FestivalWasteTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // LOGIN PAGE
        public IActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        // LOGIN POST
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                // ✅ REDIRECT TO DASHBOARD FIRST
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Invalid Email or Password";
            return View("~/Views/Account/Login.cshtml");
        }

        // REGISTER PAGE
        public IActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml");
        }

        // REGISTER POST
        [HttpPost]
        public IActionResult Register(string email, string password)
        {
            User u = new User();
            u.Username = email;
            u.Password = password;
            u.Role = "viewer";

            _context.Users.Add(u);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}