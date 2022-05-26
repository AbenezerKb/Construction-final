using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs
{
    public class BIDCreateDto
    {                
        [Required]
        public DateTime initailDate { get; set; }

        [Required]
        public DateTime finalDate { get; set; }

        [Required]
        public string WorkDescription { get; set; }

        [Required]
        public string ConBID { get; set; }

        [Required]
        public double EstimatedBID { get; set; }

        [Required]
        public double ActualCost { get; set; }              

        [Required]
        public string PenalityDescription { get; set; }

        [Required]
        public string Remark { get; set; }
        [Required]
        public string ProjectId { get; set; }

        [Required]
        public string fileName { get; set; }
    }
}
