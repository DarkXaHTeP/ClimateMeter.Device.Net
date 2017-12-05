using System;
using System.Threading.Tasks;
using ClimateMeter.Device.Net.Authentication;

namespace ClimateMeter.Device.Net
{
    public class DeviceSocket
    {
        private readonly AuthenticationTokenProvider _tokenProvider;

        public DeviceSocket(AuthenticationTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }
        
        public Task<Guid> Connect(string settingsServerBaseUrl, string deviceName, string deviceDescription)
        {
            return Task.FromResult(Guid.Empty);
        }

        public Task AddSensorReading(Guid deviceId, decimal temperature, decimal humidity)
        {
            return Task.CompletedTask;
        }
    }
}
