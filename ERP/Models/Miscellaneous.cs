using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Miscellaneous
    {
        [Key]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
