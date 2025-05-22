using Disaster_Prediction_And_Alert_System_API.BusinessLogic.DisasterPredictionAndAlert.Interface;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.ExternalApi;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.RedisCache;
using Disaster_Prediction_And_Alert_System_API.Const;
using Disaster_Prediction_And_Alert_System_API.Domain;
using Disaster_Prediction_And_Alert_System_API.Domain.Entities;
using Disaster_Prediction_And_Alert_System_API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.Text.Json;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.DisasterPredictionAndAlert.Service
{
    public class DisasterPredictionAndAlertService : IDisasterPredictionAndAlertService
    {
        private readonly AppDBContext _db;
        private readonly RedisCacheService _cache;
        private readonly IExternalApiService _external;

        public DisasterPredictionAndAlertService(AppDBContext db, RedisCacheService cache, IExternalApiService external)
        {
            _db = db;
            _cache = cache;
            _external = external;
        }

        public async Task<Region> AddRegion(RegionInfo regionInfo)
        {
            var region = new Region
            {
                Name = regionInfo.Name,
                Latitude = regionInfo.Latitude,
                Longitude = regionInfo.Longitude,
                DisasterTypes = regionInfo.DisasterTypes
            };

            await _db.Regions.AddAsync(region);
            await _db.SaveChangesAsync();
            return region;
        }

        public async Task<AlertSetting> AddAlertSetting(AlertSettingInfo settingInfo)
        {
            var setting = new AlertSetting
            {
                RegionId = settingInfo.RegionId,
                DisasterType = settingInfo.DisasterType,
                ThresholdScore = settingInfo.ThresholdScore
            };

            await _db.AlertSettings.AddAsync(setting);
            await _db.SaveChangesAsync();
            return setting;
        }

        public class FakeSmsService
        {
            public Task SendSmsAsync(string number, string message)
            {
                Console.WriteLine($"[MOCK SMS] To: {number}, Msg: {message}");
                return Task.CompletedTask;
            }
        }

        public async Task SendAlerts()
        {
            var cacheKey = "latest-risk-report";
            var cachedData = await _cache.GetAsync(cacheKey);

            if (string.IsNullOrWhiteSpace(cachedData))
            {
                Console.WriteLine("No cached risk report found.");
                return;
            }

            var reports = JsonSerializer.Deserialize<List<DisasterRiskReport>>(cachedData);

            if (reports == null || reports.Count == 0)
            {
                Console.WriteLine("No valid risk reports.");
                return;
            }

            var alertsToSend = reports.Where(r => r.AlertTriggered).ToList();

            var regianIds = alertsToSend.Select(r => r.RegionId).Distinct().ToList();

            var userMobils = await _db.Users.Where(u => regianIds.Contains(u.RegionId)).Select(u => u.Mobile).ToListAsync();

            foreach (var report in alertsToSend)
            {
                var alert = new Alert
                {
                    RegionId = report.RegionId,
                    DisasterType = report.DisasterType,
                    RiskLevel = report.RiskLevel,
                    Message = $"🚨 ALERT: {report.DisasterType.GetDisplayName()} risk is {report.RiskLevel} in region {report.RegionId}.",
                    CreatedDate = DateTime.UtcNow
                };

                await _db.Alerts.AddAsync(alert);
                await _db.SaveChangesAsync();

                Console.WriteLine($"[SENT ALERT] {alert.Message} at {alert.CreatedDate}");

                if (userMobils.Count > 0)
                {
                    var smsService = new FakeSmsService();

                    foreach (var mobile in userMobils)
                    {
                        await smsService.SendSmsAsync(mobile, alert.Message);
                    }
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task<List<Alert>> GetAllAlerts()
        {
            return await _db.Alerts.OrderByDescending(a => a.CreatedDate).ToListAsync();
        }

        public async Task<List<DisasterRiskReport>> CalculateRiskAsync()
        {
            var reports = new List<DisasterRiskReport>();
            var regions = await _db.Regions.ToListAsync();
            var currentDate = DateTime.UtcNow;

            foreach (var region in regions)
            {
                var data = await _external.GetEnvironmentalDataAsync(region);
                foreach (var type in region.DisasterTypes)
                {
                    var score = CalculateScore(type, data);
                    var riskLevel = GetRiskLevel(score);
                    var setting = await _db.AlertSettings.FirstOrDefaultAsync(s => s.RegionId == region.Id && s.DisasterType == type);
                    var triggered = setting != null && score >= setting.ThresholdScore;

                    reports.Add(new DisasterRiskReport
                    {
                        RegionId = region.Id,
                        DisasterType = type,
                        RiskScore = score,
                        RiskLevel = riskLevel,
                        AlertTriggered = triggered,
                        CreatedDate = currentDate,
                        UpdatedDate = currentDate
                    });
                }
            }

            await _db.DisasterRiskReports.AddRangeAsync(reports);
            await _db.SaveChangesAsync();

            // Save to Redis
            var json = JsonSerializer.Serialize(reports);
            await _cache.SetAsync("latest-risk-report", json);

            return reports;
        }

        private double CalculateScore(Enums.DisasterType type, Dictionary<string, double> data)
        {
            switch (type)
            {
                case Enums.DisasterType.Flood:
                    if (data.TryGetValue("rainfall", out var rainfall))
                    {
                        return Math.Min(rainfall * 2, 100);
                    }
                    break;

                case Enums.DisasterType.Earthquake:
                    if (data.TryGetValue("magnitude", out var magnitude))
                    {
                        return Math.Clamp((magnitude - 3.0) / 3.0 * 100, 0, 100);
                    }
                    break;

                case Enums.DisasterType.Wildfire:
                    if (data.TryGetValue("temperature", out var temp) && data.TryGetValue("humidity", out var humidity))
                    {
                        var score = (temp - humidity) * 2;
                        return Math.Clamp(score, 0, 100);
                    }
                    break;
            }

            return 0;
        }


        private string GetRiskLevel(double score)
        {
            string riskLevel = score >= 80 ? "High" : score >= 50 ? "Medium" : "Low";
            return riskLevel;
        }
    }
}
