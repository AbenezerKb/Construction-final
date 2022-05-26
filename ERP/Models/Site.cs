using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Site
    {
        [Key]
        public int SiteId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public int? PettyCashLimit { get; set; }

    }
}
