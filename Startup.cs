using System;
using ClimateMeter.Device.Net.Authentication;
using ClimateMeter.Device.Net.DhtReader;
using DarkXaHTeP.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClimateMeter.Device.Net
{
    public class Startup
    {   
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void Configure(IApplicationBuilder app, Device device)
        {
            app.OnExecute(() => device.Run());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // TODO Replace with ICommandLineEnvironment after issue is fixed
            if (Environment.GetEnvironmentVariable("COMMANDLINE_ENVIRONMENT") == "Development")
            {
                services.AddSingleton<IDhtReader, FakeDhtReader>();
            }
            else
            {
                services.AddSingleton<IDhtReader, Dht11Reader>();
            }
            
            services.Configure<AuthenticationSettings>(_configuration.GetSection("Authentication"));
            services.AddSingleton<AuthenticationTokenProvider>();
            
            services.Configure<DeviceSettings>(_configuration);
            services.AddSingleton<DeviceSocket>();
            services.AddSingleton<Device>();
        }
    }
}