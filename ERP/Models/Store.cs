using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }

        public string Name { get; set; } = string.Empty;

        public Site Site { get; set; }

        public int SiteId { get; set; }

    }
}
