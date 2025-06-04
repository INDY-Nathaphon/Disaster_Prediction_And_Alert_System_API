using Disaster_Prediction_And_Alert_System_API.Const;

namespace Disaster_Prediction_And_Alert_System_API.Model
{
    public class RegionAlertSettingInfo
    {
        public required string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long RegionId { get; set; }
        public long AlertSettingId { get; set; }
        public required string Message { get; set; }
        public Enums.DisasterType DisasterType { get; set; }
        public double ThresholdScore { get; set; }
    }
}
