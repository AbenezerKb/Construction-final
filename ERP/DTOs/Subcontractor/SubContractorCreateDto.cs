using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs
{
    public class SubContractorCreateDto
    {       
        [Required]
        public string SubName { get; set; }
        [Required]
        public string SubAddress { get; set; }
        [Required]
        public string SubWorkId { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
