using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs.WeeklyPlan
{
    public class WeeklyPlanValueDto
    {
        [Required]
        public int SubTaskId { get; set; }

        public int? PerformedBy { get; set; }
        public int? SubContractorId { get; set; }
    }

}