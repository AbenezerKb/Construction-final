using System.ComponentModel.DataAnnotations;


namespace ERP.DTOs
{
    public class TimeCardCreateDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public DateTime dateOfWork { get; set; }
        [Required]
        public string employeeName { get; set; }
        [Required]
        public string jobType { get; set; }
        [Required]
        public int weekNo { get; set; }
        [Required]
        public int NoOfPresents { get; set; }
        [Required]
        public int NoOfAbscents { get; set; }
        [Required]
        public int NoOfHrsPerSession { get; set; }
        [Required]
        public int totalWorkedHrs { get; set; }
        [Required]
        public double wages { get; set; }
        [Required]
        public double totalPayment { get; set; }

        //FK
        [Required]
        public string preparedByFK { get; set; }

        //FK
        [Required]
        public string approvedBy { get; set; }

        //FK
        [Required]
        public string LaborerID { get; set; }
        [Required]
        public string remark { get; set; }
    }
}
