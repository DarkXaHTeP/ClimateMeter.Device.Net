namespace ClimateMeter.Device.Net.DhtReader
{
    public interface IDhtReader
    {
        void Initialize(int pin);
        bool TryReadDhtData(out DhtData data);
    }
}