using Disaster_Prediction_And_Alert_System_API.Common.Model.Base;

namespace Disaster_Prediction_And_Alert_System_API.Common.Model.User
{
    public class UserInfo : BaseInfo
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string Mobile { get; set; }
    }
}
