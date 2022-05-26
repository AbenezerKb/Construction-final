using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class SubContractor
    {

        [Key]
        [Required]
        public string SubId { get; set; }

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
