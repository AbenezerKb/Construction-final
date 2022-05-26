using ERP.Context;
using ERP.DTOs;
using ERP.DTOs.Buy;
using ERP.Models;
using ERP.Services.BuyServices;
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
    public class BuyController : Controller
    {
        private readonly IBuyService _buyService;
        private readonly DataContext context;
        private readonly INotificationService notificationManager;

        private readonly IUserService _userService;

        public BuyController(IBuyService buyService, DataContext context, IUserService userService)
        {
            _buyService = buyService;
            this.context = context;
            this.notificationManager = notificationManager;

            _userService = userService;

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Buy>> GetById(int id)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewbuy != 1) return Forbid();

            try
            {
                Buy buy = await _buyService.GetById(id);
                return Ok(buy);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Buy>>> Get()
        {
            var buys = await context.Buys
                .OrderByDescending(buy => buy.BuyId)
                .ToListAsync();

            return Ok(buys);
        }

        [HttpPost]
        public async Task<ActionResult<List<Buy>>> GetByParams()
        {

            var buys = await _buyService.GetByCondition();

            return Ok(buys);
        }

        [HttpPost("request")]
        public async Task<ActionResult<List<Buy>>> Request(CreateBuyDTO buyDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanRequestBuy != 1) return Forbid();

            try
            {
                var buy = await _buyService.RequestBuy(buyDTO);
                return Ok(buy.BuyId);
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

        [HttpPost("check")]
        public async Task<ActionResult<Buy>> Check(CheckBuyDTO checkDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanCheckBuy != 1) return Forbid();

            try
            {
                var buy = await _buyService.CheckBuy(checkDTO);
                return Ok(buy);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("queue")]
        public async Task<ActionResult<Buy>> Queue(QueueBuyDTO queueDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanCheckBuy != 1) return Forbid();

            try
            {
                var buy = await _buyService.QueueBuy(queueDTO);
                return Ok(buy);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Buy>> Approve(ApproveBuyDTO approveDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApproveBuy != 1) return Forbid();

            try
            {
                var buy = await _buyService.ApproveBuy(approveDTO);
                return Ok(buy);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("decline")]
        public async Task<ActionResult<Buy>> Decline(ApproveBuyDTO declineDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            try
            {
                var buy = await _buyService.DeclineBuy(declineDTO);
                return Ok(buy);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("confirm")]
        public async Task<ActionResult<Buy>> Confirm(ConfirmBuyDTO confirmDTO)
        {
            try
            {
                var buy = await _buyService.ConfirmBuy(confirmDTO);
                return Ok(buy);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
