using System.IO;
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
        private readonly ICommandLineEnvironment _environment;

        public Startup(IConfiguration configuration, ICommandLineEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        
        public void Configure(IApplicationBuilder app, Device device)
        {            
            app.OnExecute(() => device.Run());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNodeServices(options =>
            {
                options.ProjectPath = Directory.GetCurrentDirectory();
            });
            
            if (_environment.IsDevelopment())
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
