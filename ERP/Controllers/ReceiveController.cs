using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.User;
using ERP.Services.ItemSiteQtyServices;
using ERP.Services.NotificationServices;
using ERP.Services.ReceiveServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceiveItem = ERP.Models.ReceiveItem;
using ERP.DTOs.Receive;

namespace ERP.Controllers
{

    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class ReceiveController : Controller
    {
        private readonly IReceiveService _receiveService;
        private readonly DataContext context;
        private readonly IItemSiteQtyService _itemSiteQtyService;
        private readonly INotificationService notificationManager;

        private readonly IUserService _userService;

        public ReceiveController(IReceiveService receiveService, IItemSiteQtyService itemSiteQtyService, DataContext context, IUserService userService)
        {
            _receiveService = receiveService;
            _itemSiteQtyService = itemSiteQtyService;
            this.context = context;
            this.notificationManager = notificationManager;

            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Receive>>> Get()
        {

            var receives = await context.Receives
                .OrderByDescending(receive => receive.ReceiveId)
                .ToListAsync();

            return Ok(receives);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Receive>> GetById(int id)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            try
            {
                Receive receive = await _receiveService.GetById(id);
                return Ok(receive);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<Receive>>> GetByParams(GetReceivesDTO getReceivesDTO)
        {

            var receives = await _receiveService.GetByCondition(getReceivesDTO);

            return Ok(receives);
        }

        [HttpGet("mysite")]
        public async Task<ActionResult<List<Receive>>> GetMySite()
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            try
            {
                var receives = await _receiveService.GetMySite();
                return Ok(receives);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("receive")]
        public async Task<ActionResult<List<Receive>>> ReceiveItem(CreateReceiveDTO receiveDTO)
        {
            try
            {
                var receive = await _receiveService.ReceiveItem(receiveDTO);
                return Ok(receive);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPost("approve")]
        public async Task<ActionResult<Receive>> ApproveReceive(ApproveReceiveDTO approveDTO)
        {
            try
            {
                var receive = await _receiveService.ApproveReceive(approveDTO);
                return Ok(receive);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
