using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;

namespace Disaster_Prediction_And_Alert_System_API.Common.Models.User
{
    public class UserWithAlertInfo : BaseInfo
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string Mobile { get; set; }
        public required string Message { get; set; }
    }
}
