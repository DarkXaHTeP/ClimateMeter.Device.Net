using DarkXaHTeP.CommandLine;
using DarkXaHTeP.Extensions.Configuration.Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClimateMeter.Device.Net
{
    class Program
    {
        static int Main(string[] args)
        {
            ICommandLineHost host = new CommandLineHostBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config
                        .AddConsul("ClimateMeter.Device")
                        .AddEnvironmentVariables("DEVICE_");
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .UseStartup<Startup>()
                .Build();

            return host.Run(args);
        }
    }
}
