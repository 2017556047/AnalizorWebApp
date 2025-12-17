using System.ComponentModel.DataAnnotations;

namespace AnalizorWebApp.Models
{
    public class Device
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string IpAddress { get; set; } = string.Empty;

        // Modbus TCP Port (genelde 502)
        public int Port { get; set; } = 502;

        // Modbus Slave / Unit ID (PAC3220 genelde 1)
        public byte UnitId { get; set; } = 1;

        public bool Enabled { get; set; } = true;
    }
}