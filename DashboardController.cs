using AnalizorWebApp.Data;
using AnalizorWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnalizorWebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;

        public DashboardController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            // =========================
            // 1️⃣ ANLIK OKUMALAR
            // =========================
            var readings = await _db.Readings
                .Include(r => r.Device)
                .OrderBy(r => r.DeviceId)
                .ToListAsync();

            // =========================
            // 2️⃣ ENERJİ HESAPLARI
            // =========================
            var now = DateTime.Now;

            var fromHour = now.AddHours(-1);
            var fromDay = now.Date;
            var fromWeek = now.AddDays(-7);
            var fromMonth = new DateTime(now.Year, now.Month, 1);

            var energySummaries = new List<DeviceEnergySummaryViewModel>();

            foreach (var device in await _db.Devices.ToListAsync())
            {
                double CalcEnergy(DateTime from)
                {
                    var snaps = _db.EnergySnapshots
                        .Where(e => e.DeviceId == device.Id && e.SnapshotTime >= from);

                    if (!snaps.Any())
                        return 0;

                    var min = snaps.Min(e => e.EnergyT1);
                    var max = snaps.Max(e => e.EnergyT1);

                    var diff = max - min;
                    return diff < 0 ? 0 : diff;
                }

                energySummaries.Add(new DeviceEnergySummaryViewModel
                {
                    DeviceId = device.Id,
                    DeviceName = device.Name,
                    Hourly = CalcEnergy(fromHour),
                    Daily = CalcEnergy(fromDay),
                    Weekly = CalcEnergy(fromWeek),
                    Monthly = CalcEnergy(fromMonth)
                });
            }

            ViewBag.EnergySummaries = energySummaries;

            return View(readings);
        }
    }
}
