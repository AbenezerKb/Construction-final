using ERP.Context;
using ERP.DTOs.User;
using ERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly DataContext context;

        public EmployeeController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = context.Employees
                .Where(employee => employee.EmployeeId == id)
                .FirstOrDefault();

            return Ok(employee);
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get()
        {
            var employees = await context.Employees
                .Include(employee => employee.UserRole) 
                .ToListAsync();

            return Ok(employees);
        }

        [HttpPost("changesite")]
        public async Task<ActionResult<Employee>> ChangeEmployeeSite(UpdateUserDTO userDTO)
        {
            var employee = context.Employees
                .Where(e => e.EmployeeId == userDTO.EmployeeId)
                .FirstOrDefault();

            if (employee == null) return NotFound("Employee Not Found.");

            employee.EmployeeSiteId = userDTO.EmployeeSiteId;

            await context.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpPost("changerole")]
        public async Task<ActionResult<Employee>> ChangeEmployeeRole(UpdateUserDTO userDTO)
        {
            var employee = context.Employees
                .Where(e => e.EmployeeId == userDTO.EmployeeId)
                .FirstOrDefault();

            if (employee == null) return NotFound("Employee Not Found.");

            employee.UserRoleId = userDTO.UserRoleId;

            await context.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpPost("update")]
        public async Task<ActionResult<Employee>> UpdateEmployee(UpdateUserDTO userDTO)
        {
            var employee = context.Employees
                .Where(e => e.EmployeeId == userDTO.EmployeeId)
                .FirstOrDefault();

            if (employee == null) return NotFound("Employee Not Found.");

            employee.Status = userDTO.Status;
            employee.FName = userDTO.FName;
            employee.MName = userDTO.MName;
            employee.LName = userDTO.LName;
            employee.Position = userDTO.Position;
            employee.EmployeeSiteId = userDTO.EmployeeSiteId;
            employee.UserRoleId = userDTO.UserRoleId;

            await context.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<UserAccount>> Approve(ApproveUserDTO approveDTO)
        {
            var employee = context.Employees.Where(employee => employee.EmployeeId == approveDTO.EmployeeId)
                .FirstOrDefault();

            if (employee == null) return NotFound("User Not Found.");

            employee.UserRoleId = approveDTO.UserRoleId;

            employee.Status = EMPLOYEESTATUS.APPROVED;

            await context.SaveChangesAsync();

            return Ok(employee);
        }
    }
}
