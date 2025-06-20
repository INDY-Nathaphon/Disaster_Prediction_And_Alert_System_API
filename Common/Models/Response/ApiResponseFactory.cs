using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;

namespace Disaster_Prediction_And_Alert_System_API.Common.Models.Response
{
    public static class ApiResponseFactory
    {
        public static ApiResponse<T> Success<T>(T data)
            => new ApiResponse<T>(data, true);

        public static ApiResponse<T> Fail<T>(string message)
            => new ApiResponse<T>(default!, false, message);

        public static ApiResponse<T> SuccessWithMessage<T>(T data, string message)
            => new ApiResponse<T>(data, true, message);

        public static ApiResponse<PagedResult<T>> Paged<T>(
            List<T> items, int page, int pageSize, int total)
        {
            var paged = new PagedResult<T>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
            return Success(paged);
        }
    }

}
