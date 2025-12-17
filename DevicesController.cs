using AnalizorWebApp.Data;
using AnalizorWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnalizorWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DevicesController : Controller
    {
        private readonly AppDbContext _db;

        public DevicesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var devices = await _db.Devices.ToListAsync();
            return View(devices);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Device model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _db.Devices.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 🔹 MODAL İÇİN
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var device = await _db.Devices.FindAsync(id);
            if (device == null)
                return NotFound();

            return PartialView("_Edit", device);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Device model)
        {
            var device = await _db.Devices.FindAsync(model.Id);
            if (device == null)
                return NotFound();

            device.Name = model.Name;
            device.IpAddress = model.IpAddress;
            device.Enabled = model.Enabled;

            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var device = await _db.Devices.FindAsync(id);
            if (device == null)
                return NotFound();

            _db.Devices.Remove(device);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
