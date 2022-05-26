
using System.Text.Json.Serialization;
using ERP.Models.Others;

namespace ERP.Models
{
    public class WeeklyResult : IAuditableEntity
    {
        public int Id { get; set; }
        public string Remark { get; set; } = string.Empty;
        public int? ApprovedBy { get; set; }
        public int WeeklyPlanId { get; set; }
        [JsonIgnore]
        public WeeklyPlan? WeeklyPlan { get; set; }
        public List<WeeklyResultValue> Results { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public double GetTotalTasksProgress()
        {
            return Results.Average(r => r.Value);
        }
    }
}