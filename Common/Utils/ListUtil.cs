namespace Disaster_Prediction_And_Alert_System_API.Common.Util
{
    public class ListUtil
    {
        public static bool IsNullOrEmpty<T>(IEnumerable<T>? list)
        {
            return list == null || !list.Any();
        }
    }
}
