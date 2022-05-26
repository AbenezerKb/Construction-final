using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs.ProjectTask
{
    public class ProjectTaskDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public bool IsSubContractorWork { get; set; } = false;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public float Budget { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
}