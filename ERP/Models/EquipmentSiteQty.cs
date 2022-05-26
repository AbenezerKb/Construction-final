using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class EquipmentSiteQty
    {
        [Key]
        public int EquipmentModelId { get; set; }
        [Key]
        public int SiteId { get; set; }

        public int Qty { get; set; }

        public int MinimumQty { get; set; }

        [ForeignKey("EquipmentModelId")]
        public EquipmentModel EquipmentModel { get; set; }

        [ForeignKey("SiteId")]
        public Site Site { get; set; }

    }
}
