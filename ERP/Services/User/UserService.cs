using ERP.Context;
using ERP.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ERP.Services.User
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRole UserRole { get; }
        public Employee Employee { get; }
       

        public UserService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            var UserAccount = context.UserAccounts
                .Where(u => u.Username == GetMyName())
                .Include(u => u.Employee)
                .ThenInclude(e => e.UserRole)
                .Include(u => u.Employee)
                .ThenInclude(e => e.EmployeeSite)
                .FirstOrDefault();

            Employee = UserAccount == null ? null : UserAccount.Employee;
            UserRole = Employee == null ? null : Employee.UserRole;
        }

        public string GetMyName()
        {
            var result = string.Empty;
            
            if (_httpContextAccessor.HttpContext != null)
            {

                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            return result;
        }

        public int GetMyId()
        {
            var result = 0;

            if (_httpContextAccessor.HttpContext != null)
            {
                var employeeId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);

                result = Convert.ToInt32(employeeId);

            }
            
            return result;
        }

        public int GetMySiteId()
        {
            var result = 0;

            if (_httpContextAccessor.HttpContext != null)
            {
                var employeeId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);

                result = Convert.ToInt32(employeeId);

            }

            return result;
        }
    }
}
