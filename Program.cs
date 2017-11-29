using System;
using System.Runtime.InteropServices;

namespace ClimateMeter.Device.Net
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            init_dht11(7);

            for (int i = 0; i < 5; i++)
            {
                if (retry_read_dht11_data(100) == 1)
                {
                    Console.WriteLine($"Temp = {get_temp()}, Humidity = {get_humidity()}");
                }
                else
                {
                    Console.WriteLine("Failed to get data");
                }
            }
        }
        
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
