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

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum SmsStatus
        {
            [Description("Success")]
            Success = 1,
            [Description("Failed")]
            Failed = 2,
            [Description("Pending")]
            Pending = 3
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum RiskLevel
        {
            [Description("Low")]
            Low = 1,
            [Description("Medium")]
            Medium = 2,
            [Description("High")]
            High = 3
        }
    }
}
