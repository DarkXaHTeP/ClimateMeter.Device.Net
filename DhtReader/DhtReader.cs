using System;

namespace ClimateMeter.Device.Net.DhtReader
{
    public class DhtReader: IDhtReader
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
            int readResult = Dht11Wrapper.retry_read_dht11_data(300);
            
            if (readResult != 1)
            {
                data = new DhtData(float.MinValue, float.MinValue);
                return false;
            }
            
            data = new DhtData(
                Dht11Wrapper.get_temp(),
                Dht11Wrapper.get_humidity()
            );

            return true;
        }
    }
}