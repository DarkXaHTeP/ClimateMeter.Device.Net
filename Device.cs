using System;
using System.Threading.Tasks;
using ClimateMeter.Device.Net.DhtReader;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClimateMeter.Device.Net
{
    public class Device
    {
        private readonly IDhtReader _dhtReader;
        private readonly DeviceSocket _socket;
        private readonly ILogger<Device> _log;
        private readonly DeviceSettings _settings;

        public Device(IDhtReader dhtReader, DeviceSocket socket, IOptions<DeviceSettings> options, ILogger<Device> log)
        {
            _dhtReader = dhtReader;
            _socket = socket;
            _settings = options.Value;
            _log = log;
        }
        
        public async Task<int> Run()
        {
            _log.LogInformation($"Starting device {_settings.DeviceName} on pin {_settings.PinNumber}");
            
            _dhtReader.Initialize(_settings.PinNumber);
            Guid deviceId = await _socket.Connect(_settings.ServerBaseUrl, _settings.DeviceName, _settings.DeviceDescription);
            
            _log.LogInformation($"Obtained device id: {deviceId}");

            while (true)
            {
                if (_dhtReader.TryReadDhtData(out var data))
                {
                    await _socket.AddSensorReading(deviceId, data.Temperature, data.Humidity);
                    
                    _log.LogInformation($"Successfully sent data to server: temperature = {data.Temperature}, Humidity = {data.Humidity}");
                }
                else
                {
                    _log.LogError("Received bad result from DHT11 Sensor");
                }

                await Task.Delay(5 * 60 * 1000); // read every 5 minutes
            }
        }
    }
}