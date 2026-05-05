using Microsoft.AspNetCore.Mvc;
using FestivalWasteTracker.Models;

namespace FestivalWasteTracker.Controllers
{
    public class WasteRecordsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public WasteRecordsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ================= ADMIN LIST =================
        public IActionResult Index()
        {
            var data = _context.WasteRecords
                .OrderByDescending(x => x.RecordId)
                .ToList();

            return View(data);
        }

        // ================= OPEN REPORT FORM =================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ================= SAVE REPORT =================
        [HttpPost]
        public IActionResult Create(WasteRecord model, IFormFile ImageFile)
        {
            try
            {
                // ⭐ VERY IMPORTANT → FORCE SAVE LOGGED IN USER
                model.ReportedBy = HttpContext.Session.GetString("User");

                // ⭐ Default Status
                model.Status = "Pending";

                // ⭐ If date not selected → take today
                if (model.CollectedDate == null)
                    model.CollectedDate = DateTime.Now;

                // ⭐ IMAGE UPLOAD
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string folder = Path.Combine(_env.WebRootPath, "uploads");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);

                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    model.ImagePath = "/uploads/" + fileName;
                }

                // ⭐ SAVE TO DATABASE
                _context.WasteRecords.Add(model);
                _context.SaveChanges();

                TempData["success"] = "Waste Report Submitted Successfully 🌱";

                // ⭐ FORCE DASHBOARD REFRESH
                return RedirectToAction("Index", "Dashboard", new { refresh = Guid.NewGuid().ToString() });
            }
            catch (Exception ex)
            {
                return Content("Error : " + ex.Message);
            }
        }

        // ================= MARK RESOLVED =================
        public IActionResult UpdateStatus(int id)
        {
            var record = _context.WasteRecords.Find(id);

            if (record != null)
            {
                record.Status = "Resolved";
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}