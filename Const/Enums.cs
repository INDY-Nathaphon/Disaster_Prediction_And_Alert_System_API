using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Disaster_Prediction_And_Alert_System_API.Const
{
    public class Enums
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum DisasterType
        {
            [Description("Flood")]
            Flood = 1,
            [Description("Earthquake")]
            Earthquake = 2,
            [Description("Wildfire")]
            Wildfire = 3
        }
    }
}
