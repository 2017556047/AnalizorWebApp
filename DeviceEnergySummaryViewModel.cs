namespace AnalizorWebApp.Models.ViewModels
{
    public class DeviceEnergySummaryViewModel
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; } = string.Empty;

        public double Hourly { get; set; }
        public double Daily { get; set; }
        public double Weekly { get; set; }
        public double Monthly { get; set; }
    }
}