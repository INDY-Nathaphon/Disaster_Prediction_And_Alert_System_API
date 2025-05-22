using Disaster_Prediction_And_Alert_System_API.Const;

namespace Disaster_Prediction_And_Alert_System_API.Domain.Entities
{
    public class Alert : BaseEntity
    {
        public long RegionId { get; set; }
        public Enums.DisasterType DisasterType { get; set; }
        public required string RiskLevel { get; set; }
        public required string Message { get; set; }
    }
}
