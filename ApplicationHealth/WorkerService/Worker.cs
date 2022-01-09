using ApplicationHealth.Domain.ViewModels;
using ApplicationHealth.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationHealth.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _scopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"Worker running at: {DateTime.Now}");
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _appService = scope.ServiceProvider.GetRequiredService<IAppDefService>();
                    var appList = _appService.GetAll();
                    foreach (var item in appList)
                    {
                        try
                        {
                            var interval = (DateTime.Now - item.LastControlDateTime).TotalMinutes;
                            if (interval > item.Interval)
                            {
                                var _httpClient = new HttpClient();
                                var response = await _httpClient.GetAsync(item.Url);
                                var res = _appService.UpdateAppStatus(item.AppDefId, DateTime.Now, response.IsSuccessStatusCode);
                                Console.WriteLine($"Name: {item.Name} Response: {response.StatusCode}");
                                _httpClient.Dispose();
                            }

                        }
                        catch (Exception ex)
                        {
                            var res = _appService.UpdateAppStatus(item.AppDefId, DateTime.Now, false);
                            Console.WriteLine($"Name: {item.Name} Response: {ex.Message}");
                            _logger.LogError($"WorkerService ==> AppDefId: {item.AppDefId} güncellenirken hata oluştu");
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
