using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class DailyLabor
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string fullName { get; set; }
        public string name { get; set; }
        public string jobTitle { get; set; }
        public double wagePerhour { get; set; }
        public DateTime date { get; set; }
        public string remarks { get; set; }


    }


    
    public class LaborDetail
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public DateTime dateOfWork { get; set; }
        public int weekNo { get; set; }
        public string dateType { get; set; }
        public string morningSession { get; set; }
        public string afternoonSession { get; set; }
        public string eveningSession { get; set; }
        public int NoOfHrsPerSession { get; set; }            
        
    }
}
