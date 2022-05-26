using ERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP.DTOs
{
    public class AssignedWorkForceCreateDto
    {
        [Required]
        public DateTime date { get; set; }
        [Required]
        public string projId { get; set; }
        [Required]
        //public string professionalName { get; set; }
        public IList<WorkForceCreateDto> ProfessionWithWork { get; set; }
        //[Required]
      //  public string profession { get; set; }
        [Required]
        public string remark { get; set; }
    }

    public class WorkForceCreateDto
    {

        [Required]
        public string assigneWorkNo { get; set; }
        [Required]
        public string EmployeeId { get; set; }
    }



}
