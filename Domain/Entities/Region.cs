﻿namespace Disaster_Prediction_And_Alert_System_API.Domain.Entities
{
    public class Region : BaseEntity
    {
        public required string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
