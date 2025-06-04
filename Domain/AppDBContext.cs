using Disaster_Prediction_And_Alert_System_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Disaster_Prediction_And_Alert_System_API.Domain
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<AlertLog> AlertLogs { get; set; }
        public DbSet<AlertSetting> AlertSettings { get; set; }
        public DbSet<DisasterRiskReport> DisasterRiskReports { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAlertSettingMap> UserAlertSettingMaps { get; set; }
    }
}
