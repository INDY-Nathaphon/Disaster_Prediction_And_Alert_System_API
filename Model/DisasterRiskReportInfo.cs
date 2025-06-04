using Disaster_Prediction_And_Alert_System_API.Const;

namespace Disaster_Prediction_And_Alert_System_API.Model
{
    public class DisasterRiskReportInfo : BaseInfo
    {
        public long RegionId { get; set; }
        public Enums.DisasterType DisasterType { get; set; }
        public double RiskScore { get; set; }
        public required string RiskLevel { get; set; }
        public bool AlertTriggered { get; set; }
    }
}
