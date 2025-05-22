using Disaster_Prediction_And_Alert_System_API.Domain.Entities;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.ExternalApi
{
    public interface IExternalApiService
    {
        Task<Dictionary<string, double>> GetEnvironmentalDataAsync(Region region);
    }
}
