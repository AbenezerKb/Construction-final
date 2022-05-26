using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class UserAccount
    {
        [Key]
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
        
        public Employee Employee { get; set; }

        public int? EmployeeId { get; set; }

    }
}
