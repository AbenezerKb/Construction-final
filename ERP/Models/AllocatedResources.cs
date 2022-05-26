using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class AllocatedResources
    {


        [Key]
        [Required]
        public string allocatedResourcesNo { get; set; }
        public DateTime date { get; set; }
        public string projId { get; set; }
        public string  itemName { get; set; }
        public string  unit { get; set; }
        public string remark { get; set; }

    }
}
