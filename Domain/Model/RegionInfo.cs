using System.ComponentModel.DataAnnotations;
using static Disaster_Prediction_And_Alert_System_API.Const.Enums;

namespace Disaster_Prediction_And_Alert_System_API.Domain.Model
{
    public class RegionInfo : BaseInfo
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}
