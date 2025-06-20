using Disaster_Prediction_And_Alert_System_API.Common.Constant;
using Disaster_Prediction_And_Alert_System_API.Common.Model.Base;
using Disaster_Prediction_And_Alert_System_API.Exceptions;

namespace Disaster_Prediction_And_Alert_System_API.Common.Validation
{
    public class FilterValidator
    {
        public static void Validate(BaseFilter filter)
        {
            if (filter == null)
            {
                throw new AppException(AppErrorCode.ValidationError, "Filter cannot be null.");
            }

            if (!filter.IsAllItems)
            {
                if (filter.Page < 1)
                {
                    throw new AppException(AppErrorCode.ValidationError, "Page must be greater than 0.");
                }

                if (filter.PageSize < 1 || filter.PageSize > 1000)
                {
                    throw new AppException(AppErrorCode.ValidationError, "Page size must be between 1 and 1000.");
                }
            }

            if (filter.StartDate.HasValue && filter.EndDate.HasValue && (filter.EndDate < filter.StartDate))
            {
                throw new AppException(AppErrorCode.ValidationError, "End date must be greater than start date.");
            }
        }
    }
}
