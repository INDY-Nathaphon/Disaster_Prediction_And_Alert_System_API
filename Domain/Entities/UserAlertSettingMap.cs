namespace Disaster_Prediction_And_Alert_System_API.Domain.Entities
{
    public class UserAlertSettingMap : BaseEntity
    {
        public long UserId { get; set; }
        public long AlertSettingId { get; set; }
    }
}
