namespace Disaster_Prediction_And_Alert_System_API.Domain.Model
{
    public class UserWithAlertInfo : BaseInfo
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string Mobile { get; set; }
        public required string Message { get; set; }
    }
}
