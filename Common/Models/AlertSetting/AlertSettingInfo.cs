﻿using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;
using Disaster_Prediction_And_Alert_System_API.Const;

namespace Disaster_Prediction_And_Alert_System_API.Common.Models.AlertSetting
{
    public class AlertSettingInfo : BaseInfo
    {
        public long RegionId { get; set; }
        public Enums.DisasterType DisasterType { get; set; }
        public double ThresholdScore { get; set; }
        public required string Message { get; set; }
    }
}
