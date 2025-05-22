namespace Disaster_Prediction_And_Alert_System_API.Domain.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
