using Disaster_Prediction_And_Alert_System_API.Common.Constant;

namespace Disaster_Prediction_And_Alert_System_API.Exceptions
{
    public class AppException : Exception
    {
        public AppErrorCode MessageId { get; }

        public int? StatusCode { get; set; }

        public AppException(AppErrorCode messageId)
            : this(messageId, null, null)
        {
        }

        public AppException(AppErrorCode messageId, string? message)
            : this(messageId, null, message)
        {
        }

        public AppException(AppErrorCode messageId, int? statusCode)
           : this(messageId, statusCode, null)
        {
        }

        public AppException(AppErrorCode messageId, int? statusCode, string? message)
            : base(message)
        {
            MessageId = messageId;
            StatusCode = statusCode;
        }
    }
}
