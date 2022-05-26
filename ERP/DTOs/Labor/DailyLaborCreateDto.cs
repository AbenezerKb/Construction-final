using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs
{
    public class DailyLaborCreateDto
    {
        [Required]
        public string fullName { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string jobTitle { get; set; }
        [Required]
        public double wagePerhour { get; set; }
        [Required]
        public DateTime date { get; set; }
        [Required]
        public string remarks { get; set; }
    }
}
