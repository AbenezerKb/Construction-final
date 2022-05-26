using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        public int Status { get; set; } = 0;
        /*Status Values: 
           0: Registered
           1: Approved
           2: Deleted/Fired
       */
        public string FName { get; set; } = string.Empty;

        public string MName { get; set; } = string.Empty;

        public string LName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public Site EmployeeSite { get; set; }

        public int? EmployeeSiteId { get; set; }

        public UserRole UserRole { get; set; }

        public int UserRoleId { get; set; }

    }
}
