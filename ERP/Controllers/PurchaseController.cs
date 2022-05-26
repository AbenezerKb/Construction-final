using ERP.Context;
using ERP.DTOs;
using ERP.DTOs.Purchase;
using ERP.Models;
using ERP.Services.NotificationServices;
using ERP.Services.PurchaseServices;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using PurchaseItem = ERP.Models.PurchaseItem;

namespace ERP.Controllers
{

    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class PurchaseController : Controller
    {
        private readonly IPurchaseService _purchaseService;
        private readonly DataContext context;
        private readonly INotificationService notificationManager;

        private readonly IUserService _userService;

        public PurchaseController(IPurchaseService purchaseService, DataContext context, IUserService userService)
        {
            _purchaseService = purchaseService;
            this.context = context;
            this.notificationManager = notificationManager;

            _userService = userService;            

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetById(int id)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();
            try
            {
                Purchase purchase = await _purchaseService.GetById(id);
                return Ok(purchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Purchase>>> Get()
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var purchases = await context.Purchases
                .Include(purchase => purchase.ReceivingSite)
                .OrderByDescending(purchase => purchase.PurchaseId)
                .ToListAsync();

            return Ok(purchases);
        }

        [HttpPost]
        public async Task<ActionResult<List<Purchase>>> GetByParams(GetPurchasesDTO getPurchasesDTO)
        {

            var purchases = await _purchaseService.GetByCondition(getPurchasesDTO);

            return Ok(purchases);
        }

        [HttpPost("request/material")]
        public async Task<ActionResult<int>> RequestMaterial(CreateMaterialPurchaseDTO purchaseDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanRequestPurchase != 1) return Forbid();

            try
            {
                var purchase = await _purchaseService.RequestMaterial(purchaseDTO);
                return Ok(purchase.PurchaseId);
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

        [HttpPost("request/equipment")]
        public async Task<ActionResult<List<Purchase>>> RequestEquipment (CreateEquipmentPurchaseDTO purchaseDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanRequestPurchase != 1) return Forbid();

            try
            {
                var purchase = await _purchaseService.RequestEquipment(purchaseDTO);
                return Ok(purchase.PurchaseId);
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
        public async Task<ActionResult<Purchase>> Approve(ApprovePurchaseDTO approveDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            try
            {
                var purchase = await _purchaseService.ApprovePurchase(approveDTO);
                return Ok(purchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("decline")]
        public async Task<ActionResult<Purchase>> Decline(ApprovePurchaseDTO declineDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            try
            {
                var purchase = await _purchaseService.DeclinePurchase(declineDTO);
                return Ok(purchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("check")]
        public async Task<ActionResult<Purchase>> Check(CheckPurchaseDTO checkDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanCheckPurchase != 1) return Forbid();

            try
            {
                var purchase = await _purchaseService.CheckPurchase(checkDTO);
                return Ok(purchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("queue")]
        public async Task<ActionResult<Purchase>> Queue(CheckPurchaseDTO checkDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            try
            {
                var purchase = await _purchaseService.QueuePurchase(checkDTO);
                return Ok(purchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost("requestqueued")]
        public async Task<ActionResult<Purchase>> RequestQueuedPurchase(CheckPurchaseDTO checkDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanCheckPurchase != 1) return Forbid();

            try
            {
                var purchase = await _purchaseService.RequestQueuedPurchase(checkDTO);
                return Ok(purchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("confirm")]
        public async Task<ActionResult<Purchase>> Confirm(ConfirmPurchaseDTO confirmDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            try
            {
                var purchase = await _purchaseService.ConfirmPurchase(confirmDTO);
                return Ok(purchase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}