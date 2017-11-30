namespace ClimateMeter.Device.Net.DhtReader
{
    public class FakeDhtReader: IDhtReader
    {
        public void Initialize(int pin)
        {
            // Do nothing here
        }

        public bool TryReadDhtData(out DhtData data)
        {
            data = new DhtData(26.3f, 55f);
            return true;
        }
    }
}