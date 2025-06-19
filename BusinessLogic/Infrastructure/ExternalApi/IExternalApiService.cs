namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.ExternalApi
{
    public interface IExternalApiService
    {
        Task<Dictionary<string, double>> GetEnvironmentalDataAsync(double latitude, double longitude);
    }
}
