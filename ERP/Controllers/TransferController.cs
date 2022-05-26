using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.NotificationServices;
using ERP.Services.TransferServices;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TransferItem = ERP.Models.TransferItem;

namespace ERP.Controllers
{

    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class TransferController : Controller
    {
        private readonly ITransferService _transferService;

        private readonly IUserService _userService;


        public TransferController(ITransferService transferService, IUserService userService)
        {
            _transferService = transferService;

            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transfer>> Get(int id)
        {
            try
            {
                Transfer transfer = await _transferService.GetById(id);
                
                if( _userService.UserRole.IsAdmin || _userService.UserRole.IsFinance ||
                    (_userService.UserRole.CanViewTransfer && _userService.Employee.EmployeeSiteId == transfer.SendSiteId) ||
                    (_userService.UserRole.CanViewTransfer && _userService.Employee.EmployeeSiteId == transfer.ReceiveSiteId) ||
                    _userService.Employee.EmployeeId == transfer.RequestedById)
                    return Ok(transfer);

                return Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            
        }

        [HttpPost]
        public async Task<ActionResult<List<Transfer>>> GetByParams(GetTransfersDTO getTransfersDTO)
        {

            var transfers = await _transferService.GetByCondition(getTransfersDTO);

            return Ok(transfers);
        }

        [HttpPost("request/equipment")]
        public async Task<ActionResult<int>> RequestEquipment(CreateEquipmentTransferDTO transferDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanRequestTransfer)
                return Forbid();

            try
            {
                var transfer = await _transferService.RequestEquipment(transferDTO);
                return Ok(transfer.TransferId);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("request/material")]
        public async Task<ActionResult<List<Transfer>>> RequestMaterial(CreateMaterialTransferDTO transferDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanRequestTransfer)
                return Forbid();

            try
            {
                var transfer = await _transferService.RequestMaterial(transferDTO);
                return Ok(transfer.TransferId);
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
        public async Task<ActionResult<Transfer>> Approve(ApproveTransferDTO approveDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanApproveTransfer)
                return Forbid();

            try
            {
                var transfer = await _transferService.ApproveTransfer(approveDTO);
                return Ok(transfer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("decline")]
        public async Task<ActionResult<Transfer>> Decline(DeclineTransferDTO declineDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanApproveTransfer)
                return Forbid();

            try
            {
                var transfer = await _transferService.DeclineTransfer(declineDTO);
                return Ok(transfer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("send")]
        public async Task<ActionResult<Transfer>> Send(SendTransferDTO sendDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanSendTransfer)
                return Forbid();

            try
            {
                var transfer = await _transferService.SendTransfer(sendDTO);
                return Ok(transfer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost("receive")]
        public async Task<ActionResult<Transfer>> Receive(ReceiveTransferDTO receiveDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanReceiveTransfer)
                return Forbid();

            try
            {
                var transfer = await _transferService.ReceiveTransfer(receiveDTO);
                return Ok(transfer);
            }

            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

    }
}
