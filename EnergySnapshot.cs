using System;

namespace AnalizorWebApp.Models
{
    public class EnergySnapshot
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public double EnergyT1 { get; set; }

        public DateTime SnapshotTime { get; set; }

        public Device? Device { get; set; }
    }
}