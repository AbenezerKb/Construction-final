using System.ComponentModel.DataAnnotations;
namespace ERP.DTOs
{
    public class AllocatedResourcesCreateDto
    {
        [Required]
        public DateTime date { get; set; }
        [Required]
        public string projId { get; set; }
        [Required]
        public string itemName { get; set; }
        [Required]
        public string unit { get; set; }
        [Required]
        public string remark { get; set; }
    }
}
