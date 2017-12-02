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
                .Range(0, 5)
                .Select(_ => ReadSingleResult())
                .Where(res => res.HasValue)
                .Select(res => res.Value)
                .ToList();

            if (!results.Any())
            {
                data = new DhtData(float.MinValue, float.MinValue);
                
                return false;
            }

            float temperature = results
                .Select(res => res.Temperature)
                .Average(res => res);
            
            float humidity = results
                .Select(res => res.Humidity)
                .Average(res => res);

            data = new DhtData(temperature, humidity);
            
            return true;
        }

        private DhtData? ReadSingleResult()
        {
            Thread.Sleep(1000); // a short delay to avoid issues with sensor called too often
            int readResult = Dht11Wrapper.retry_read_dht11_data(300);
            
            if (readResult != 1)
            {
                _log.LogWarning("Receiived bad result from Sensor");
                
                return null;
            }
            
            var data = new DhtData(
                Dht11Wrapper.get_temp(),
                Dht11Wrapper.get_humidity()
            );
            
            _log.LogInformation($"Read next values: temperature = {data.Temperature}, humidity = {data.Humidity}");

            return data;
        }
    }
}