using Disaster_Prediction_And_Alert_System_API.Const;

namespace Disaster_Prediction_And_Alert_System_API.Model
{
    public class AlertSettingInfo
    {
        public long RegionId { get; set; }
        public Enums.DisasterType DisasterType { get; set; }
        public double ThresholdScore { get; set; }

    }
}
