using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Contract
    {
        [Key]
        [Required]
        public string ConId { get; set; }
        
        [Required]
        public string ConType { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string ConGiver { get; set; }

        [Required]
        public string ConReciever { get; set; }

        [Required]
        public ICollection<SubContractingWork> SubConstructWorkDetail { get; set; }

        [Required]
        public String Unit { get; set; }

        [Required]
        public double Cost { get; set; }
       
    }

    public class SubContractingWork
    {        
        public string SubcontractingWorkID { get; set; }
        public string unit { get; set; }
        public double unitPrice { get; set; }
        public double priceWithVat { get; set; }
        public string ContractID { get; set; }
    }
}
