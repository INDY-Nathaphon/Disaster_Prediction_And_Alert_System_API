using Disaster_Prediction_And_Alert_System_API.Domain.Entities;
using Disaster_Prediction_And_Alert_System_API.Model;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.DisasterPredictionAndAlert.Interface
{
    public interface IDisasterPredictionAndAlertFacadeService
    {
        public Task<Region> AddRegion(RegionInfo region);
        public Task<AlertSetting> AddAlertSetting(AlertSettingInfo setting);
        public Task SendAlerts();
        public Task<List<Alert>> GetAllAlerts();
        public Task<List<DisasterRiskReport>> CalculateRiskAsync();
    }
}
