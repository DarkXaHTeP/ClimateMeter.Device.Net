using System;
using System.Threading.Tasks;
using ClimateMeter.Device.Net.Authentication;
using ClimateMeter.Device.Net.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets;
using Microsoft.Extensions.Logging;

namespace ClimateMeter.Device.Net
{
    public class DeviceSocket
    {
        private readonly AuthenticationTokenProvider _tokenProvider;
        private readonly ILoggerFactory _loggerFactory;
        private HubConnection _connection;

        public DeviceSocket(AuthenticationTokenProvider tokenProvider, ILoggerFactory loggerFactory)
        {
            _tokenProvider = tokenProvider;
            _loggerFactory = loggerFactory;
        }
        
        public async Task<Guid> Connect(string serverBaseUrl, string deviceName, string deviceDescription)
        {
            InitConnection(serverBaseUrl);

            var registeredTaskSource = new TaskCompletionSource<Guid>();
            
            _connection.On<Guid>("DeviceRegistered", id =>
            {
                registeredTaskSource.SetResult(id);
            });

            await _connection.StartAsync();

            await _connection.InvokeAsync("RegisterDevice", deviceName, deviceDescription);

            Guid deviceId = await registeredTaskSource.Task;
            
            return deviceId;
        }

        private void InitConnection(string serverBaseUrl)
        {
            var builder = new HubConnectionBuilder()
                .WithJWTAuthentication(() => _tokenProvider.ObtainJwtToken())
                .WithUrl($"{serverBaseUrl}/socket/device")
                .WithTransport(TransportType.WebSockets)
                .WithMessagePackProtocol()
                .WithLoggerFactory(_loggerFactory);

            _connection = builder.Build();
        }

        public async Task AddSensorReading(Guid deviceId, decimal temperature, decimal humidity)
        {
            await _connection.InvokeAsync("AddSensorReading", deviceId, temperature, humidity);
        }
    }
}
