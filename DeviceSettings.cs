namespace ClimateMeter.Device.Net
{
    public class DeviceSettings
    {
        public string DeviceName { get; set; }
        public string DeviceDescription { get; set; }
        public int PinNumber { get; set; }
        public string ServerBaseUrl { get; set; }
    }
}