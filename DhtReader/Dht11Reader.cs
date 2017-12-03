using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;

namespace ClimateMeter.Device.Net.DhtReader
{
    public class Dht11Reader: IDhtReader
    {
        private readonly INodeServices _nodeServices;
        private readonly ILogger<Dht11Reader> _log;
        private int _pin;

        public Dht11Reader(INodeServices nodeServices, ILogger<Dht11Reader> log)
        {
            _nodeServices = nodeServices;
            _log = log;
        }
        
        public void Initialize(int pin)
        {
            _pin = pin;
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
            
            return (float) Math.Round(total / count * 10) / 10f;
        }

        private DhtData? ReadSingleResult(uint attempt)
        {
            Thread.Sleep(800); // a short delay to avoid issues with sensor called too often

            try
            {
                var data = _nodeServices.InvokeAsync<DhtData>("./DhtReader/readDhtData", 4).GetAwaiter().GetResult();
                
                Console.WriteLine($"{data.Temperature}|--|{data.Humidity}");

                return data;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Unable to read data from node-dht");
                return null;
            }
        }
    }
}