using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs
{
    public class LaborDetailCreateDto
    {
        [Required]
        public DateTime dateOfWork { get; set; }
        [Required]
        public int weekNo { get; set; }
        [Required]
        public string dateType { get; set; }
        [Required]
        public string morningSession { get; set; }
        [Required]
        public string afternoonSession { get; set; }
        [Required]
        public string eveningSession { get; set; }
        [Required]
        public int NoOfHrsPerSession { get; set; }

        //FK
   //     [Required]
     //   public string LaborerID { get; set; }
    }
}
