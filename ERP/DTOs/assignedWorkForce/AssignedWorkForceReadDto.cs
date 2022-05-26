using ERP.Models;
namespace ERP.DTOs
{
    public class AssignedWorkForceReadDto
    {
        public string assigneWorkForceNo { get; set; }
        public DateTime date { get; set; }
        public string projId { get; set; }
        // public string professionalName { get; set; }
        public IList<WorkForceReadDto> ProfessionWithWork { get; set; }
        //   public string profession { get; set; }
        public string remark { get; set; }
    }


    public class WorkForceReadDto
    {        
        public string WokrkForceID { get; set; }
        public string assigneWorkForceNo { get; set; }
        public string EmployeeId { get; set; }
    }




}
