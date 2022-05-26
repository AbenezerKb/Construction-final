using ERP.Context;
using ERP.DTOs.BulkPurchase;
using ERP.Models;
using ERP.Services.BulkPurchaseServices;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class BulkPurchaseController : Controller
    {
        private readonly IBulkPurchaseService _bulkPurchaseService;
        private readonly DataContext context;
        private readonly INotificationService notificationManager;

        private readonly IUserService _userService;

        public BulkPurchaseController(IBulkPurchaseService bulkPurchaseService, DataContext context, IUserService userService)
        {
            _bulkPurchaseService = bulkPurchaseService;
            this.context = context;
            this.notificationManager = notificationManager;

            _userService = userService;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BulkPurchase>> GetById(int id)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewBulkPurchase != 1) return Forbid();
            try
            {
                BulkPurchase purchase = await _bulkPurchaseService.GetById(id);
                return Ok(purchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<BulkPurchase>>> Get()
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var bulkPurchases = await context.BulkPurchases
                .OrderByDescending(bulk => bulk.BulkPurchaseId)
                .ToListAsync();

            return Ok(bulkPurchases);
        }

        [HttpPost("request")]
        public async Task<ActionResult<BulkPurchase>> Request(RequestBulkPurchaseDTO requestDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            try
            {
                var bulkPurchase = await _bulkPurchaseService.RequestBulkPurchase(requestDTO);
                return Ok(bulkPurchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("approve")]
        public async Task<ActionResult<BulkPurchase>> Approve(ApproveBulkPurchaseDTO approveDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            try
            {
                var bulkPurchase = await _bulkPurchaseService.ApproveBulkPurchase(approveDTO);
                return Ok(bulkPurchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("decline")]
        public async Task<ActionResult<BulkPurchase>> Decline(ApproveBulkPurchaseDTO declineDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            try
            {
                var bulkPurchase = await _bulkPurchaseService.DeclineBulkPurchase(declineDTO);
                return Ok(bulkPurchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("confirm")]
        public async Task<ActionResult<BulkPurchase>> Confirm(ConfirmBulkPurchaseDTO confirmDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            try
            {
                var bulkPurchase = await _bulkPurchaseService.ConfirmBulkPurchase(confirmDTO);
                return Ok(bulkPurchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
