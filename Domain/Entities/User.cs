namespace Disaster_Prediction_And_Alert_System_API.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string Mobile { get; set; }
        public long RegionId { get; set; }
    }
}
