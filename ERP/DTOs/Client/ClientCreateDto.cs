using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs
{
    public class ClientCreateDto
    {
        [Required]
        public string clientName { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string contractorId { get; set; }
        [Required]
        public string estimatedDuration { get; set; }
        [Required]
        public string estimatedCost { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public DateTime dateOfContract { get; set; }
        [Required]
        public string attachmentOfContract { get; set; }
        [Required]
        public string remarks { get; set; }

    }
}
