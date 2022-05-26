using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Incident
    {

        [Key]
        [Required]
        public string incidentNo { get; set; }        
        public string incidentName { get; set; }
        public string proID { get; set; }
        public string empName { get; set; }
        public DateTime date { get; set; }
        public string Description { get; set; }
    }
}
