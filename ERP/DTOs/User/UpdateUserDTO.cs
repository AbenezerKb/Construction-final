namespace ERP.DTOs.User
{
    public class UpdateUserDTO
    {
        public int EmployeeId { get; set; }
        
        public int Status { get; set; }
        
        public string FName { get; set; } = string.Empty;

        public string MName { get; set; } = string.Empty;

        public string LName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public int EmployeeSiteId { get; set; }
        
        public int UserRoleId { get; set; }
    }
}
