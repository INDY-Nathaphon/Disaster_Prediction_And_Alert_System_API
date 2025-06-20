namespace Disaster_Prediction_And_Alert_System_API.Common.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class ErrorMetaAttribute : System.Attribute
    {
        public string Message { get; }

        public ErrorMetaAttribute(string message)
        {
            Message = message;
        }
    }
}