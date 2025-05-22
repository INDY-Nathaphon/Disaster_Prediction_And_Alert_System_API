namespace Disaster_Prediction_And_Alert_System_API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Request started at: {DateTime.Now}");

            await _next(context);

            Console.WriteLine($"Request ended at: {DateTime.Now}");
        }
    }
}
