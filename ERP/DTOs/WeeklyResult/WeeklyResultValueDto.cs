using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs.WeeklyResult
{
    public class WeeklyResultValueDto
    {
        [Required]
        public int Value { get; set; }
        [Required]
        public int SubTaskId { get; set; }

    }
}