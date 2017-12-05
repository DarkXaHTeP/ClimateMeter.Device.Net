namespace ClimateMeter.Device.Net.DhtReader
{
    public struct DhtData
    {
        public DhtData(decimal temperature, decimal humidity)
        {
            Temperature = temperature;
            Humidity = humidity;
        }
        
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
    }
}