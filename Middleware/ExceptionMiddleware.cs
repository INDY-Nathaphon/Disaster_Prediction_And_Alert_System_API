using Disaster_Prediction_And_Alert_System_API.Common.ApiResponse;
using Disaster_Prediction_And_Alert_System_API.Common.Constant;
using Disaster_Prediction_And_Alert_System_API.Common.Helper;
using Disaster_Prediction_And_Alert_System_API.Exceptions;
using System.Net;
using System.Text.Json;

namespace Disaster_Prediction_And_Alert_System_API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception caught in middleware");

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            AppErrorCode errorCode;
            string message;

            if (exception is AppException appEx)
            {
                errorCode = appEx.MessageId;
                message = string.IsNullOrEmpty(appEx.Message)
                            ? ErrorHelper.GetMessage(errorCode)
                            : appEx.Message;
                statusCode = appEx.StatusCode ?? (int)HttpStatusCode.InternalServerError;
            }
            else
            {
                errorCode = AppErrorCode.InternalError;
                message = ErrorHelper.GetMessage(errorCode);
                statusCode = (int)HttpStatusCode.InternalServerError;
            }

            var errorResponse = new ApiResponse<object>
            {
                MessageId = (int)errorCode,
                Message = message,
                Success = false,
                Data = null
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(errorResponse);
            return context.Response.WriteAsync(result);
        }
    }
}
