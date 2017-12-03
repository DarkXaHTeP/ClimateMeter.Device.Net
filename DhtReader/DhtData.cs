namespace ClimateMeter.Device.Net.DhtReader
{
    public struct DhtData
    {
        public DhtData(float temperature, float humidity)
        {
            Temperature = temperature;
            Humidity = humidity;
        }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
    }
}