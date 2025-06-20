using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Disaster_Prediction_And_Alert_System_API.Common.Models.Region
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
