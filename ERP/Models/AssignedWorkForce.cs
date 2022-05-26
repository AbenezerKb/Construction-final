
using ERP.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class AssignedWorkForce
    {            
        [Key]
        [Required]
        public string assigneWorkForceNo  { get; set; }
        public DateTime date { get; set; }
        public string projId { get; set; }
       // public string professionalName { get; set; }
        public IList<WorkForce> ProfessionWithWork { get; set; }
        public string remark { get; set; }

    }

    public class WorkForce
    {
        [Key]
        [Required]
        public string WokrkForceID { get; set; }
        public string assigneWorkForceNo { get; set; }
        public string EmployeeId { get; set; }
    }
}
