using ApplicationHealth.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace ApplicationHealth.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var cs = @"Host=localhost;Port=5432;Database=AppHealth;Username=postgres;Password=pass";
                    services.AddDatabase(cs).AddRepositories().AddEntityServices();
                    services.AddHostedService<Worker>();

                }).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                }).UseNLog();  // NLog: setup NLog for Dependency injection;;
    }
}
