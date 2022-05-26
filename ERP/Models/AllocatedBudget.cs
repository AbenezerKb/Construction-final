using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class AllocatedBudget
    {

        [Key]
        [Required]  
        public string  Id { get; set; }
        public DateTime date { get; set; }
        public string projectId { get; set; }
        public string activity { get; set; }
        public double amount { get; set; }
        public string contingency { get; set; }
        public string preparedBy { get; set; }
        public string ApprovedBy { get; set; }

    }
}
