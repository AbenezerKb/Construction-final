using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.IssueServices;
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
    public class IssueController : Controller
    {
        private readonly IIssueService _issueService;
        private readonly IUserService _userService;

        public IssueController( IIssueService issueService, IUserService userService)
        {
            _issueService = issueService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> Get(int id)
        {

            try
            {
                var issue = await _issueService.GetById(id);

                if (_userService.UserRole.IsAdmin || _userService.UserRole.IsFinance ||
                        (_userService.UserRole.CanViewIssue && _userService.Employee.EmployeeSiteId == issue.SiteId) ||
                        _userService.Employee.EmployeeId == issue.RequestedById)
                    return Ok(issue);

                return Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<Issue>>> GetByCondition(GetIssuesDTO getIssuesDTO)
        {
            var issues = await _issueService.GetByCondition(getIssuesDTO);

            return Ok(issues);
        }

        [HttpPost("request")]
        public async Task<ActionResult<int>> Post(CreateIssueDTO issueDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanRequestIssue)
                return Forbid();

            try
            {
                var issue = await _issueService.RequestIssue(issueDTO);
                return Ok(issue.IssueId);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Issue>> Approve(ApproveIssueDTO approveDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanApproveIssue)
                return Forbid();

            try
            {
                var issue = await _issueService.ApproveIssue(approveDTO);
                return Ok(issue.IssueId);
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
        public async Task<ActionResult<Issue>> Decline(DeclineIssueDTO declineDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanApproveIssue)
                return Forbid();

            try
            {
                var issue = await _issueService.DeclineIssue(declineDTO);
                return Ok(issue.IssueId);
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

        [HttpPost("hand")]
        public async Task<ActionResult<Issue>> Hand(HandIssueDTO handDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanHandIssue)
                return Forbid();

            try
            {
                var issue = await _issueService.HandIssue(handDTO);
                return Ok(issue);
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
