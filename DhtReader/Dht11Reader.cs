using System;
using System.Collections.Generic;
using System.Linq;

namespace ClimateMeter.Device.Net.DhtReader
{
    public class Dht11Reader: IDhtReader
    {
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
                .Select(res => res.Temperature)
                .Average(res => res);

            data = new DhtData(temperature, humidity);
            
            return true;
        }

        private DhtData? ReadSingleResult()
        {
            int readResult = Dht11Wrapper.retry_read_dht11_data(300);
            
            if (readResult != 1)
            {
                return null;
            }
            
            return new DhtData(
                Dht11Wrapper.get_temp(),
                Dht11Wrapper.get_humidity()
            );
        }
    }
}