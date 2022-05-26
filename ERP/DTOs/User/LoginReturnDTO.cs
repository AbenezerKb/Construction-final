using ERP.Models;

namespace ERP.DTOs.User
{
    public class LoginReturnDTO
    {
            public string token { get; set; }

            public Employee employee { get; set; }

            public Models.UserRole userRole { get; set; }

            public string username { get; set; }

    }
}
