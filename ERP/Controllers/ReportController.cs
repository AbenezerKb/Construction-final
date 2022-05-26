using ERP.Context;
using ERP.DTOs;
using ERP.DTOs.Report;
using ERP.Models;
using ERP.Services.ReportServices;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IUserService _userService;

        public ReportController( IReportService reportService, IUserService userService)
        {
            _reportService = reportService;
            _userService = userService;
        }

        [HttpPost("receive")]
        public async Task<ActionResult<ReportReturnDTO<Transfer>>> GetReceives(ReceiveReportDTO receiveDTO)
        {
            if (!_userService.UserRole.IsAdmin || !_userService.UserRole.IsFinance)
                return Forbid();

            var result = await _reportService.GetReceiveReport(receiveDTO);

            return Ok(result);
        }

        [HttpPost("purchase")]
        public async Task<ActionResult<ReportReturnDTO<Transfer>>> GetPurchases(PurchaseReportDTO purchaseDTO)
        {
            if (!_userService.UserRole.IsAdmin || !_userService.UserRole.IsFinance)
                return Forbid();

            var result = await _reportService.GetPurchaseReport(purchaseDTO);

            return Ok(result);
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<ReportReturnDTO<Transfer>>> GetTransfers(TransferReportDTO transferDTO)
        {
            if (!_userService.UserRole.IsAdmin || !_userService.UserRole.IsFinance)
                return Forbid();

            var result = await _reportService.GetTansferReport(transferDTO);

            return Ok(result);
        }

        [HttpPost("issue")]
        public async Task<ActionResult<ReportReturnDTO<Transfer>>> GetIssues(IssueReportDTO issueDTO)
        {
            if (!_userService.UserRole.IsAdmin || !_userService.UserRole.IsFinance)
                return Forbid();

            var result = await _reportService.GetIssueReport(issueDTO);

            return Ok(result);
        }

        [HttpPost("borrow")]
        public async Task<ActionResult<ReportReturnDTO<Transfer>>> GetBorrows(BorrowReportDTO borrowDTO)
        {
            if (!_userService.UserRole.IsAdmin || !_userService.UserRole.IsFinance)
                return Forbid();

            var result = await _reportService.GetBorrowReport(borrowDTO);

            return Ok(result);
        }

        [HttpPost("maintenance")]
        public async Task<ActionResult<ReportReturnDTO<Transfer>>> GetMaintenances(MaintenanceReportDTO maintenanceDTO)
        {
            if (!_userService.UserRole.IsAdmin || !_userService.UserRole.IsFinance)
                return Forbid();

            var result = await _reportService.GetMaintenanceReport(maintenanceDTO);

            return Ok(result);
        }

        [HttpPost("general")]
        public async Task<ActionResult<GeneralReportReturnDTO>> Post(GeneralReportDTO reportDTO)
        {
            if (!_userService.UserRole.IsAdmin || !_userService.UserRole.IsFinance)
                return Forbid();

            return Ok(await _reportService.GetGeneralReport(reportDTO));
        }

    }
}
