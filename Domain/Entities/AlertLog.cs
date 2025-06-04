using Disaster_Prediction_And_Alert_System_API.Const;
using static Disaster_Prediction_And_Alert_System_API.Const.Enums;

namespace Disaster_Prediction_And_Alert_System_API.Domain.Entities
{
    public class AlertLog : BaseEntity
    {
        public long RegionId { get; set; }
        public long UserId { get; set; }
        public Enums.DisasterType DisasterType { get; set; }
        public required string RiskLevel { get; set; }
        public required string MobileNo { get; set; }
        public SmsStatus SmsStatus { get; set; }
        public required string Message { get; set; }
    }
}
