using AnalizorWebApp.Data;
using AnalizorWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalizorWebApp.Workers
{
    public class EnergySnapshotWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EnergySnapshotWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = GetDelayUntilNextHour();
                await Task.Delay(delay, stoppingToken);

                await TakeSnapshot();
            }
        }

        private static TimeSpan GetDelayUntilNextHour()
        {
            var now = DateTime.Now;

            var nextHour = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                0,
                0
            ).AddHours(1);

            return nextHour - now;
        }

        private async Task TakeSnapshot()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var readings = await db.Readings.ToListAsync();

            foreach (var r in readings)
            {
                db.EnergySnapshots.Add(new EnergySnapshot
                {
                    DeviceId = r.DeviceId,
                    EnergyT1 = r.EnergyT1,
                    SnapshotTime = DateTime.Now
                });
            }

            await db.SaveChangesAsync();
        }
    }
}
