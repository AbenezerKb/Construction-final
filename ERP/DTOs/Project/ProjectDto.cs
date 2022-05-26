using System.ComponentModel.DataAnnotations;
namespace ERP.DTOs.Project
{
    public class ProjectDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int ManagerId { get; set; }
        [Required]
        public int CoordinatorId { get; set; }
        [Required]
        public int SiteId { get; set; }


    }
}
