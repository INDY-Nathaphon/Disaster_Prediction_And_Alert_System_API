﻿using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterRiskReport.Interface;

namespace Disaster_Prediction_And_Alert_System_API.Scheduler
{
    public class AlertJobService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AlertJobService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var disasterRiskReportFacadeService = scope.ServiceProvider.GetRequiredService<IDisasterRiskReportFacadeService>();

                    await disasterRiskReportFacadeService.CreateDisasterRiskReport();

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
