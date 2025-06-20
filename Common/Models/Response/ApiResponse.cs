namespace Disaster_Prediction_And_Alert_System_API.Common.Models.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int? MessageId { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(T data, bool success)
        {
            Data = data;
            Success = success;
        }

        public ApiResponse(T data, bool success, string message)
        {
            Data = data;
            Success = success;
            Message = message;
        }
    }
}
