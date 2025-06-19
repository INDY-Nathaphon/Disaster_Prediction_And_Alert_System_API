using StackExchange.Redis;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.RedisCache
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConfiguration config)
        {
            var connStr = config["Redis:ConnectionString"] ?? string.Empty;
            var redis = ConnectionMultiplexer.Connect(connStr);
            _db = redis.GetDatabase();
        }

        public async Task SetAsync(string key, string value)
        {
            // กำหนด expire 15 นาที
            await _db.StringSetAsync(key, value, TimeSpan.FromMinutes(15));
        }

        public async Task<string> GetAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return string.Empty;
            }
            return value.ToString();
        }
    }
}
