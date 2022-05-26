using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs.SubTask
{

    public class SubTaskDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]

        public int Priority { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public double Budget { get; set; }
        [Required]
        public double Progress { get; set; }
        [Required]
        public string Remark { get; set; } = string.Empty;
        [Required]
        public int TaskId { get; set; }
    }
}