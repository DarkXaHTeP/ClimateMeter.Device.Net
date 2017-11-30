using DarkXaHTeP.CommandLine;
using Microsoft.Extensions.Logging;

namespace ClimateMeter.Device.Net
{
    class Program
    {
        static int Main(string[] args)
        {
            ICommandLineHost host = new CommandLineHostBuilder()
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
