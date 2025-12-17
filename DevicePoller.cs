using AnalizorWebApp.Data;
using AnalizorWebApp.Hubs;
using AnalizorWebApp.Models;
using AnalizorWebApp.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AnalizorWebApp.Workers
{
    public class DevicePoller : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<DevicePoller> _logger;
        private readonly IHubContext<LiveDataHub> _hubContext;

        private readonly TimeSpan _interval = TimeSpan.FromSeconds(2);

        public DevicePoller(
            IServiceProvider services,
            ILogger<DevicePoller> logger,
            IHubContext<LiveDataHub> hubContext)
        {
            _services = services;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var modbus = scope.ServiceProvider.GetRequiredService<ModbusService>();

                    var devices = await db.Devices
                        .Where(d => d.Enabled)
                        .ToListAsync(stoppingToken);

                    foreach (var device in devices)
                    {
                        try
                        {
                            var (v1, v2, v3, i1, i2, i3, p1, p2, p3)
                                = modbus.ReadStage1(device.IpAddress);

                            var energy = modbus.ReadTotalActiveEnergyT1(device.IpAddress);

                            var reading = await db.Readings
                                .FirstOrDefaultAsync(r => r.DeviceId == device.Id, stoppingToken);

                            if (reading == null)
                            {
                                reading = new Reading
                                {
                                    DeviceId = device.Id
                                };
                                db.Readings.Add(reading);
                            }

                            reading.Timestamp = DateTime.Now;
                            reading.V1 = v1;
                            reading.V2 = v2;
                            reading.V3 = v3;
                            reading.I1 = i1;
                            reading.I2 = i2;
                            reading.I3 = i3;
                            reading.P1 = p1;
                            reading.P2 = p2;
                            reading.P3 = p3;
                            reading.EnergyT1 = energy;

                            await db.SaveChangesAsync(stoppingToken);

                            // 🔴 CANLI DASHBOARD GÜNCELLEME
                            await _hubContext.Clients.All.SendAsync("ReceiveLiveData", new
                            {
                                deviceId = device.Id,
                                v1 = reading.V1,
                                v2 = reading.V2,
                                v3 = reading.V3,
                                i1 = reading.I1,
                                i2 = reading.I2,
                                i3 = reading.I3,
                                p1 = reading.P1,
                                p2 = reading.P2,
                                p3 = reading.P3,
                                energyT1 = reading.EnergyT1,
                                timestamp = reading.Timestamp.ToString("HH:mm:ss")
                            }, stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Cihaz okuma hatası: {IP}", device.IpAddress);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "DevicePoller genel hata");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
