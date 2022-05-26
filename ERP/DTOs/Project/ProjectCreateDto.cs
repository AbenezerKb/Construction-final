using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs
{
    public class ProjectCreateDto
    {
        [Required]
        public string proName { get; set; }
        [Required]
        public DateTime dateOfStart { get; set; }
        [Required]
        public DateTime dateOfComplete { get; set; }
        [Required]
        public string proCoordinator { get; set; }
        [Required]
        public string proManager { get; set; }
        [Required]
        public string status { get; set; }

        [Required]
        public string siteId { get; set; }
    }
}
