using System;
using System.Threading.Tasks;
using JokeGenerator.Application;
using JokeGenerator.ConsolePresentation;
using JokesApiClient;
using JokesApiClient.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Http.Client;

namespace JokeGenerator
{
    public sealed class Program
    {
        public static async Task Main()
        {
            // In an application should be just one place were we should configure the application. It's a host
            // Here I build the host. Basically here I'm configuring container, configuration 

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            var path = configuration["Logging:FilePath"];
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path)
                .CreateLogger();

            await using var serviceProvider = new ServiceCollection()
                .AddLogging(_ => _.AddSerilog())
                .AddSingleton<IPresentationBehavior, ConsoleMaster>()
                .AddSingleton<ConsoleWriter>()
                .AddTransient<ApplicationService>()
                .AddTransient<IWebClientFactory, WebClientFactory>()
                .AddTransient<IJokeWebApiClient, JokeWebApiClient>()
                .AddOptions()
                .Configure<JokesSettings>(configuration.GetSection(nameof(JokesSettings)))
                .BuildServiceProvider(true);
            
            try
            {
                Log.Information("Starting");
                await serviceProvider.GetService<ApplicationService>().Run();
                Log.Information("Finished");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host builder error");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}