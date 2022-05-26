using System.ComponentModel.DataAnnotations;


namespace ERP.Models
{
    public class SubContractWork
    {

        [Key]
        [Required]
        public string subconractingid { get; set; }
        public string workName { get; set; }
        public string remarks { get; set; }
    }
}
