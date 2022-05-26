using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs
{
    public class SubContractWorkCreateDto
    {
        
        [Required]
        public string workName { get; set; }
        [Required]
        public string remarks { get; set; }
    }
}
