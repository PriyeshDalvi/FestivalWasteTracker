using Microsoft.AspNetCore.Mvc;
using FestivalWasteTracker.Models;

namespace FestivalWasteTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string festival, string refresh)
        {
           
            // ⭐ VERY IMPORTANT — FORCE NO CACHE (REAL TIME UPDATE)
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var role = HttpContext.Session.GetString("Role");
            var user = HttpContext.Session.GetString("User");

            // ⭐ HARD CODE FESTIVAL LIST
            ViewBag.Festivals = new List<string>
            {
                "Ganesh Chaturthi",
                "Diwali",
                "Navratri",
                "Holi",
                "Durga Puja",
                "Eid",
                "Christmas",
                "Local Fair"
            };

            ViewBag.SelectedFestival = festival;

            var query = _context.WasteRecords.AsQueryable();

            // ⭐ FILTER BY FESTIVAL
            if (!string.IsNullOrEmpty(festival) && festival != "All Festivals")
            {
                query = query.Where(x => x.EventName == festival);
            }

            if (role == "admin")
            {
                ViewBag.TotalWasteRecords = query.Count();

                ViewBag.PendingWaste = query
                    .Where(x => x.Status == "Pending")
                    .Count();

                ViewBag.ResolvedWaste = query
                    .Where(x => x.Status == "Resolved")
                    .Count();

                ViewBag.TotalWasteKg = query
                    .Sum(x => x.QuantityKg) ?? 0;

                ViewBag.RecentReports = query
                    .OrderByDescending(x => x.RecordId)
                    .Take(5)
                    .ToList();
            }
            else
            {
                var userQuery = query.Where(x => x.ReportedBy == user);

                ViewBag.TotalWasteRecords = userQuery.Count();

                ViewBag.PendingWaste = userQuery
                    .Where(x => x.Status == "Pending")
                    .Count();

                ViewBag.ResolvedWaste = userQuery
                    .Where(x => x.Status == "Resolved")
                    .Count();

                ViewBag.TotalWasteKg = userQuery
                    .Sum(x => x.QuantityKg) ?? 0;
            }

            // ⭐ CHART DATA
            ViewBag.PlasticWaste = query
                .Where(x => x.WasteType == "Plastic")
                .Sum(x => x.QuantityKg) ?? 0;

            ViewBag.OrganicWaste = query
                .Where(x => x.WasteType == "Organic")
                .Sum(x => x.QuantityKg) ?? 0;

            ViewBag.PaperWaste = query
                .Where(x => x.WasteType == "Paper")
                .Sum(x => x.QuantityKg) ?? 0;

            return View();
        }
    }
}