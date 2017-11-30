using System;
using System.Threading;
using ClimateMeter.Device.Net.DhtReader;
using DarkXaHTeP.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClimateMeter.Device.Net
{
    public class Startup
    {   
        public void Configure(IApplicationBuilder app, IDhtReader dhtReader, ILogger<Startup> log)
        {
            app.OnExecute(() =>
            {
                log.LogInformation("Starting execution");
                
                dhtReader.Initialize(7);

                for (int i = 0; i < 5; i++)
                {
                    if (dhtReader.TryReadDhtData(out var dhtData))
                    {
                        log.LogInformation($"Temperature = { dhtData.Temperature }, Humidity = { dhtData.Humidity }");
                    }
                    else
                    {
                        log.LogError("Unable to read dht data");
                    }
                    
                    Thread.Sleep(1000);
                }

                return 0;
            });
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
                services.AddSingleton<IDhtReader, DhtReader.DhtReader>();
            }
        }
    }
}