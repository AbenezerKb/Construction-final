using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs
{
    public class IncidentCreateDto
    {
        [Required]
        public string incidentName { get; set; }

        [Required]
        public string proID { get; set; }
        
        [Required]
        public DateTime date { get; set; }

        [Required]
        public string empName { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
