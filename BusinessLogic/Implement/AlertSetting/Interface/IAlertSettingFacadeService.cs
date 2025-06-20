using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Base;
using Disaster_Prediction_And_Alert_System_API.Common.Models.AlertSetting;
using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterPredictionAndAlert.Interface
{
    public interface IAlertSettingFacadeService : IBaseService<AlertSettingInfo, BaseFilter>
    {
    }
}
