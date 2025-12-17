using System;

namespace AnalizorWebApp.Models
{
    public class Reading
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }

        public double V1 { get; set; }
        public double V2 { get; set; }
        public double V3 { get; set; }

        public double I1 { get; set; }
        public double I2 { get; set; }
        public double I3 { get; set; }

        public double P1 { get; set; }
        public double P2 { get; set; }
        public double P3 { get; set; }

        public double EnergyT1 { get; set; }

        public int DeviceId { get; set; }
        public Device? Device { get; set; }
    }
}
