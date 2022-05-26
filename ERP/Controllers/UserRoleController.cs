using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : Controller
    {
        private readonly DataContext context;

        public UserRoleController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserRole>> Get(int id)
        {
            var role = context.UserRoles.Where(role => role.RoleId == id)
               .FirstOrDefault();

            if (role == null) return NotFound("Role Not Found.");

            return Ok(role);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserRole>>> Get()
        {
            var roles = await context.UserRoles.ToListAsync();

            return Ok(roles);
        }

        // [Authorize(Roles = "Employee")]
        [HttpPost("add")]
        public async Task<ActionResult<UserRole>> AddNew(UserRole role)
        {
            UserRole userRole = new();
            userRole.Role = role.Role;

            userRole.CanRequestBorrow = role.CanRequestBorrow;
            userRole.CanApproveBorrow = role.CanApproveBorrow;
            userRole.CanHandBorrow = role.CanHandBorrow;
            userRole.CanReturnBorrow = role.CanReturnBorrow;
            userRole.CanViewBorrow = role.CanViewBorrow;

            userRole.CanRequestBuy = role.CanRequestBuy;
            userRole.CanCheckBuy = role.CanCheckBuy;
            userRole.CanApproveBuy = role.CanApproveBuy;
            userRole.CanConfirmBuy = role.CanConfirmBuy;
            userRole.CanViewBuy = role.CanViewBuy;

            userRole.CanReceive = role.CanReceive;
            userRole.CanApproveReceive = role.CanApproveReceive;
            userRole.CanViewReceive = role.CanViewReceive;

            userRole.CanEditUser = role.CanEditUser;

            userRole.CanRequestPurchase = role.CanRequestPurchase;
            userRole.CanCheckPurchase = role.CanCheckPurchase;
            userRole.CanApprovePurchase = role.CanApprovePurchase;
            userRole.CanConfirmPurchase = role.CanConfirmPurchase;
            userRole.CanViewPurchase = role.CanViewPurchase;

            userRole.CanRequestBulkPurchase = role.CanRequestBulkPurchase;
            userRole.CanApproveBulkPurchase = role.CanApproveBulkPurchase;
            userRole.CanConfirmBulkPurchase = role.CanConfirmBulkPurchase;
            userRole.CanViewBulkPurchase = role.CanViewBulkPurchase;

            userRole.CanFixMaintenance = role.CanFixMaintenance;
            userRole.CanApproveMaintenance = role.CanApproveMaintenance;
            userRole.CanRequestMaintenance = role.CanRequestMaintenance;
            userRole.CanViewMaintenance = role.CanViewMaintenance;

            userRole.CanRequestIssue = role.CanRequestIssue;
            userRole.CanApproveIssue = role.CanApproveIssue;
            userRole.CanHandIssue = role.CanHandIssue;
            userRole.CanViewIssue = role.CanViewIssue;

            userRole.CanRequestTransfer = role.CanRequestTransfer;
            userRole.CanApproveTransfer = role.CanApproveTransfer;
            userRole.CanReceiveTransfer = role.CanReceiveTransfer;
            userRole.CanSendTransfer = role.CanSendTransfer;
            userRole.CanViewTransfer = role.CanViewTransfer;

            userRole.CanGetStockNotification = role.CanGetStockNotification;
            userRole.IsFinance = role.IsFinance;

            context.UserRoles.Add(userRole);

            await context.SaveChangesAsync();

            return Ok(userRole);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("edit")]
        public async Task<ActionResult<UserRole>> EditRole(UserRole role)
        {
            var userRole = context.UserRoles.Where(role => role.RoleId == role.RoleId)
               .FirstOrDefault();

            if (userRole == null) return NotFound("Role Not Found.");

            userRole.Role = role.Role;

            userRole.CanRequestBorrow = role.CanRequestBorrow;
            userRole.CanApproveBorrow = role.CanApproveBorrow;
            userRole.CanHandBorrow = role.CanHandBorrow;
            userRole.CanReturnBorrow = role.CanReturnBorrow;
            userRole.CanViewBorrow = role.CanViewBorrow;

            userRole.CanRequestBuy = role.CanRequestBuy;
            userRole.CanCheckBuy = role.CanCheckBuy;
            userRole.CanApproveBuy = role.CanApproveBuy;
            userRole.CanConfirmBuy = role.CanConfirmBuy;
            userRole.CanViewBuy = role.CanViewBuy;

            userRole.CanReceive = role.CanReceive;
            userRole.CanApproveReceive = role.CanApproveReceive;
            userRole.CanViewReceive = role.CanViewReceive;

            userRole.CanEditUser = role.CanEditUser;

            userRole.CanRequestPurchase = role.CanRequestPurchase;
            userRole.CanCheckPurchase = role.CanCheckPurchase;
            userRole.CanApprovePurchase = role.CanApprovePurchase;
            userRole.CanConfirmPurchase = role.CanConfirmPurchase;
            userRole.CanViewPurchase = role.CanViewPurchase;

            userRole.CanRequestBulkPurchase = role.CanRequestBulkPurchase;
            userRole.CanApproveBulkPurchase = role.CanApproveBulkPurchase;
            userRole.CanConfirmBulkPurchase = role.CanConfirmBulkPurchase;
            userRole.CanViewBulkPurchase = role.CanViewBulkPurchase;

            userRole.CanFixMaintenance = role.CanFixMaintenance;
            userRole.CanApproveMaintenance = role.CanApproveMaintenance;
            userRole.CanRequestMaintenance = role.CanRequestMaintenance;
            userRole.CanViewMaintenance = role.CanViewMaintenance;

            userRole.CanRequestIssue = role.CanRequestIssue;
            userRole.CanApproveIssue = role.CanApproveIssue;
            userRole.CanHandIssue = role.CanHandIssue;
            userRole.CanViewIssue = role.CanViewIssue;

            userRole.CanRequestTransfer = role.CanRequestTransfer;
            userRole.CanApproveTransfer = role.CanApproveTransfer;
            userRole.CanReceiveTransfer = role.CanReceiveTransfer;
            userRole.CanSendTransfer = role.CanSendTransfer;
            userRole.CanViewTransfer = role.CanViewTransfer;

            userRole.CanGetStockNotification = role.CanGetStockNotification;
            userRole.IsFinance = role.IsFinance;

            await context.SaveChangesAsync();

            return Ok(userRole);
        }
    }
}
