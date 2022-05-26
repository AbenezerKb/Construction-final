using System.Text.Json.Serialization;
using ERP.Models.Others;

namespace ERP.Models
{
    public class WeeklyPlanValue : IAuditableEntity
    {
        public int Id { get; set; }
        public int? PerformedBy { get; set; }
        public int? SubContractorId { get; set; }
        public int? SubTaskId { get; set; }
        public SubTask? SubTask { get; set; }
        public int WeeklyPlanId { get; set; }
        [JsonIgnore]
        public WeeklyPlan? WeeklyPlan { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsAssignedForSubContractor() => SubContractorId != null;
    }

}