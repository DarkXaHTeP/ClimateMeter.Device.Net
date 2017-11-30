namespace ClimateMeter.Device.Net.DhtReader
{
    public struct DhtData
    {
        public DhtData(float temperature, float humidity)
        {
            Temperature = temperature;
            Humidity = humidity;
        }
        public float Temperature { get; }
        public float Humidity { get; }
    }
}