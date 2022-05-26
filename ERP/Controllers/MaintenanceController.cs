using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.MaintenanceServices;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class MaintenanceController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IUserService _userService;


        public MaintenanceController( IMaintenanceService maintenanceService, IUserService userService)
        {
            _maintenanceService = maintenanceService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Maintenance>> Get(int id)
        {
            try
            {
                var maintenance = await _maintenanceService.GetById(id);

                if (_userService.UserRole.IsAdmin || _userService.UserRole.IsFinance || 
                    (_userService.UserRole.CanViewMaintenance && _userService.Employee.EmployeeSiteId == maintenance.SiteId) ||
                    _userService.Employee.EmployeeId == maintenance.RequestedById)
                return Ok(maintenance);

                return Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Maintenance>>> Get()
        {

            var maintenance = await _maintenanceService.GetByCondition();

            return Ok(maintenance);
        }

        [HttpGet("assets")]
        public async Task<ActionResult<List<EquipmentAsset>>> GetItems(int modelId)
        {
            var items = await _maintenanceService.GetItems(modelId);

            return Ok(items);
        }

        [HttpPost("request")]
        public async Task<ActionResult<int>> Post(CreateMaintenanceDTO maintenanceDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanRequestMaintenance)
                return Forbid();

            try
            {
                var maintenance = await _maintenanceService.RequestMaintenance(maintenanceDTO);
                return Ok(maintenance.MaintenanceId);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Maintenance>> Approve(ApproveMaintenanceDTO approveDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanApproveMaintenance)
                return Forbid();

            try
            {
                var maintenance = await _maintenanceService.ApproveMaintenance(approveDTO);
                return Ok(maintenance);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("decline")]
        public async Task<ActionResult<Maintenance>> Decline(ApproveMaintenanceDTO declineDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanApproveMaintenance)
                return Forbid();

            try
            {
                var maintenance = await _maintenanceService.DeclineMaintenance(declineDTO);
                return Ok(maintenance);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("fix")]
        public async Task<ActionResult<Maintenance>> fix(FixMaintenanceDTO fixDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanFixMaintenance)
                return Forbid();

            try
            {
                var maintenance = await _maintenanceService.FixMaintenance(fixDTO);
                return Ok(maintenance);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
