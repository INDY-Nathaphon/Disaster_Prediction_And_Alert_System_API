using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.ExternalApi;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.RedisCache;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterRiskReport.Interface;
using Disaster_Prediction_And_Alert_System_API.Const;
using Disaster_Prediction_And_Alert_System_API.Domain;
using Disaster_Prediction_And_Alert_System_API.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static Disaster_Prediction_And_Alert_System_API.Const.Enums;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterRiskReport.Service
{
    public class DisasterRiskReportService : IDisasterRiskReportService
    {
        private readonly AppDBContext _db;
        private readonly RedisCacheService _cache;
        private readonly IExternalApiService _external;

        public DisasterRiskReportService(AppDBContext db, RedisCacheService cache, IExternalApiService external)
        {
            _db = db;
            _cache = cache;
            _external = external;
        }

        public async Task CreateDisasterRiskReport()
        {
            var currentDate = DateTime.UtcNow;
            var reports = new List<Domain.Entities.DisasterRiskReport>();
            var alertLogs = new List<Domain.Entities.AlertLog>();

            var regionAlertSettingInfo = await (from a in _db.AlertSettings
                                                join r in _db.Regions on a.RegionId equals r.Id
                                                select new RegionAlertSettingInfo
                                                {
                                                    DisasterType = a.DisasterType,
                                                    Latitude = r.Latitude,
                                                    Longitude = r.Longitude,
                                                    RegionId = r.Id,
                                                    AlertSettingId = a.Id,
                                                    Name = r.Name,
                                                    ThresholdScore = a.ThresholdScore,
                                                    Message = a.Message
                                                }).ToListAsync();

            foreach (var item in regionAlertSettingInfo)
            {
                var data = await _external.GetEnvironmentalDataAsync(item.Latitude, item.Longitude);

                var score = CalculateScore(item.DisasterType, data);
                var riskLevel = GetRiskLevel(score);
                var triggered = score >= item.ThresholdScore;

                // Skip if score is 0
                if (score == 0)
                {
                    continue;
                }

                reports.Add(new Domain.Entities.DisasterRiskReport
                {
                    RegionId = item.RegionId,
                    DisasterType = item.DisasterType,
                    RiskScore = score,
                    RiskLevel = riskLevel,
                    AlertTriggered = triggered,
                    CreatedDate = currentDate,
                    UpdatedDate = currentDate
                });

                // Update cache if triggered
                if (triggered)
                {
                    //create alert notification
                    var userWithAlert = await (from u in _db.UserAlertSettingMaps
                                               join u2 in _db.Users on u.UserId equals u2.Id
                                               where u.AlertSettingId == item.AlertSettingId
                                               select new UserWithAlertInfo
                                               {
                                                   Id = u2.Id,
                                                   Name = u2.Name,
                                                   Mobile = u2.Mobile,
                                                   Email = u2.Email,
                                                   Message = item.Message
                                               }
                                               ).ToListAsync();

                    foreach (var user in userWithAlert)
                    {
                        var alertLog = new Domain.Entities.AlertLog()
                        {
                            Message = user.Message,
                            MobileNo = user.Mobile,
                            RegionId = item.RegionId,
                            UserId = user.Id,
                            SmsStatus = SmsStatus.Pending,
                            RiskLevel = riskLevel,
                            DisasterType = item.DisasterType,
                            CreatedDate = currentDate,
                            UpdatedDate = currentDate
                        };

                        alertLogs.Add(alertLog);
                    }

                    var json = JsonSerializer.Serialize(reports);
                    await _cache.SetAsync("latest-risk-report", json);
                }
            }

            await _db.DisasterRiskReports.AddRangeAsync(reports);
            await _db.AlertLogs.AddRangeAsync(alertLogs);
            await _db.SaveChangesAsync();
        }

        public async Task SendDisasterRiskReport()
        {
            var json = await _cache.GetAsync("latest-risk-report");

            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            var pendingAlerts = await (from a in _db.AlertLogs
                                       where a.SmsStatus == SmsStatus.Pending || a.SmsStatus == SmsStatus.Failed
                                       select a).ToListAsync();

            foreach (var item in pendingAlerts)
            {
                var result = SendSmsAsync(item.MobileNo, item.Message);

                if (!result)
                {
                    item.SmsStatus = SmsStatus.Failed;
                    item.UpdatedDate = DateTime.UtcNow;
                }
                else
                {
                    item.SmsStatus = SmsStatus.Success;
                    item.UpdatedDate = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync();
        }

        #region private method

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

        private bool SendSmsAsync(string number, string message)
        {
            Console.WriteLine($"[MOCK SMS] To: {number}, Msg: {message}");
            return true;
        }

        private string GetRiskLevel(double score)
        {
            string riskLevel = score >= 80 ? "High" : score >= 50 ? "Medium" : "Low";
            return riskLevel;
        }

        #endregion
    }
}
