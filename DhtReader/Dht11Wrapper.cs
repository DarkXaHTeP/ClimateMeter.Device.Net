using System.Runtime.InteropServices;

namespace ClimateMeter.Device.Net.DhtReader
{
    public static class Dht11Wrapper
    {
        [DllImport("libdht11.so", EntryPoint = "init_dht11")]
        public static extern int init_dht11(int pin);
        
        [DllImport("libdht11.so", EntryPoint = "retry_read_dht11_data")]
        public static extern int retry_read_dht11_data(int max_retries);
        
        [DllImport("libdht11.so", EntryPoint = "get_temp")]
        public static extern float get_temp();
        
        [DllImport("libdht11.so", EntryPoint = "get_humidity")]
        public static extern float get_humidity();
    }
}