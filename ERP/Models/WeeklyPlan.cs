using System.Text.Json.Serialization;
using ERP.Models.Others;

namespace ERP.Models
{
    public class WeeklyPlan : IAuditableEntity
    {
        public int Id { get; set; }
        public DateTime WeekStartDate { get; set; }
        public int WeekNo { get; set; }
        public string Remark { get; set; } = string.Empty;
        public List<WeeklyPlanValue> PlanValues { get; set; } = new();
        [JsonIgnore]
        public WeeklyResult? WeeklyResult { get; set; }
        public int ProjectId { get; set; }
        [JsonIgnore]
        public Project? Project { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}