using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace ClimateMeter.Device.Net.DhtReader
{
    public class Dht11Reader: IDhtReader
    {
        private readonly ILogger<Dht11Reader> _log;

        public Dht11Reader(ILogger<Dht11Reader> log)
        {
            _log = log;
        }
        
        public void Initialize(int pin)
        {
            if (Dht11Wrapper.init_dht11(pin) != 1)
            {
                throw new Exception("Unable to initialize DHT11 sensor");
            }
        }

        public bool TryReadDhtData(out DhtData data)
        {
            List<DhtData> results = Enumerable
                .Range(0, 17)
                .Select(_ => ReadSingleResult(0))
                .Where(res => res.HasValue)
                .Select(res => res.Value)
                .ToList();

            if (!results.Any())
            {
                data = new DhtData(float.MinValue, float.MinValue);
                
                return false;
            }

            IEnumerable<float> temperatures = results
                .Select(res => res.Temperature);

            float temperature = ProcessSensorReading(temperatures);

            IEnumerable<float> humidities = results
                .Select(res => res.Humidity);

            float humidity = ProcessSensorReading(humidities);

            data = new DhtData(temperature, humidity);
            
            return true;
        }

        private float ProcessSensorReading(IEnumerable<float> results)
        {
            var valueGroups = results
                .GroupBy(res => res)
                .OrderByDescending(res => res.Count())
                .Take(2)
                .ToList();

            var count = valueGroups
                .Select(group => group.Count())
                .Sum();
            
            Console.WriteLine($"Taking average from { String.Join(", ", valueGroups.Select(gr => $"{gr.Key} (count - {gr.Count()})")) }");

            var total = valueGroups
                .Select(gr => gr.Key * gr.Count())
                .Sum();
            
            return total / count;
        }

        private DhtData? ReadSingleResult(uint attempt)
        {
            Thread.Sleep(1000); // a short delay to avoid issues with sensor called too often
            int readResult = Dht11Wrapper.retry_read_dht11_data(300);
            
            if (readResult != 1)
            {
                _log.LogWarning("Received bad result from Sensor");
                
                return null;
            }
            
            var data = new DhtData(
                Dht11Wrapper.get_temp(),
                Dht11Wrapper.get_humidity()
            );

            if (Math.Abs(data.Humidity - 20f) < 0.05f && Math.Abs(data.Temperature - 22f) < 0.05f && attempt < 7)
            {
                Console.WriteLine("!! Received 22 and 20, Skipping...");
                
                return ReadSingleResult(attempt + 1);
            }
            
            Console.WriteLine($"{data.Temperature}|--|{data.Humidity}");

            return data;
        }
    }
}