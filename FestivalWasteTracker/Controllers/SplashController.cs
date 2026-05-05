using Microsoft.AspNetCore.Mvc;

namespace FestivalWasteTracker.Controllers
{
    public class SplashController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}