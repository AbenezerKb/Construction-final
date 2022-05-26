
using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs.WeeklyPlan
{
    public class WeeklyPlanDto
    {
        [Required]
        public DateTime WeekStartDate { get; set; }
        public List<WeeklyPlanValueDto> PlanValues { get; set; } = new();
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public string Remark { get; set; } = string.Empty;

    }
}