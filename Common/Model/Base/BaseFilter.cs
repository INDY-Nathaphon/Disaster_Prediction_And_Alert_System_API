namespace Disaster_Prediction_And_Alert_System_API.Common.Model.Base
{
    public class BaseFilter
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; } = null;
        public bool IsAllItems { get; set; }

        // สำหรับการเรียงลำดับ
        public string? SortBy { get; set; } = null;  // เช่น "CreatedDate", "Name"
        public string? OrderBy { get; set; } = "asc"; // "asc" หรือ "desc"

        // สำหรับกรองตามวันที่
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
    }
}
