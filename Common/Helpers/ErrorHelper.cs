using Disaster_Prediction_And_Alert_System_API.Common.Attribute;
using Disaster_Prediction_And_Alert_System_API.Common.Constant;

namespace Disaster_Prediction_And_Alert_System_API.Common.Helpers
{
    public static class ErrorHelper
    {
        public static string GetMessage(AppErrorCode code)
        {
            var member = typeof(AppErrorCode).GetMember(code.ToString()).FirstOrDefault();
            var attr = member?.GetCustomAttributes(typeof(ErrorMetaAttribute), false)
                              .FirstOrDefault() as ErrorMetaAttribute;

            return attr?.Message ?? "Unknown error.";
        }

        public static int GetCode(AppErrorCode code) => (int)code;
    }
}
