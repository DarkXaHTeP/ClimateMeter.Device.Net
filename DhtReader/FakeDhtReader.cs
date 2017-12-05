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
            data = new DhtData(26.3m, 55m);
            return true;
        }
    }
}