using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs.WeeklyResult
{
    public class WeeklyResultDto
    {

        [Required]
        public int WeeklyPlanId { get; set; }

        [Required]
        public List<WeeklyResultValueDto> Results { get; set; } = new();

        [Required]
        public string Remark { get; set; } = string.Empty;
    }

}

