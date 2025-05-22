namespace Disaster_Prediction_And_Alert_System_API.Domain
{
    public class AppSettings
    {

        public ConnectionStringSettings ConnectionSetting { get; set; } = new();
        public RedisSettings Redis { get; set; } = new();
        public WeatherApiSettings WeatherApi { get; set; } = new();

        public class ConnectionStringSettings
        {
            public string DbConnection { get; set; } = string.Empty;
        }

        public class RedisSettings
        {
            public string ConnectionString { get; set; } = string.Empty;
            public int TokenExpiryMinutes { get; set; }
        }

        public class WeatherApiSettings
        {
            public string Key { get; set; } = string.Empty;
        }
    }

    // Extension method for binding configuration
    public static class ServiceExtensions
    {
        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
        }
    }
}
