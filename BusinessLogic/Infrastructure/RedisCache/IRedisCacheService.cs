namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.RedisCache
{
    public interface IRedisCacheService
    {
        Task SetAsync(string key, string value);
        Task<string> GetAsync(string key);
    }
}
