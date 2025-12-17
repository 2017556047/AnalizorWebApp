using AnalizorWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnalizorWebApp.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Reading> Readings { get; set; }
        public DbSet<EnergySnapshot> EnergySnapshots { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Device -> Readings (CASCADE DELETE)
            builder.Entity<Reading>()
                .HasOne(r => r.Device)
                .WithMany()
                .HasForeignKey(r => r.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Device -> EnergySnapshots (CASCADE DELETE)
            builder.Entity<EnergySnapshot>()
                .HasOne(e => e.Device)
                .WithMany()
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
