using System.ComponentModel.DataAnnotations;
using ERP.Models;

namespace ERP.DTOs
{
    public class ContractCreateDto
    {
        [Required]
        public string ConType { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string ConGiver { get; set; }

        [Required]
        public string ConReciever { get; set; }

        [Required]
        public ICollection<SubContractingWorkCreateDto> SubConstructWorkDetail { get; set; }

        [Required]
        public string Unit { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public string Attachement { get; set; }

    }

    public class SubContractingWorkCreateDto
    {
        [Required]
        public string unit { get; set; }
        [Required]
        public double unitPrice { get; set; }
        [Required]
        public double priceWithVat { get; set; }
        public string ContractID { get; set; }
    }


}
