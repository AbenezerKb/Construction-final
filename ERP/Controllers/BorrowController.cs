using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.BorrowServices;
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
    public class BorrowController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService, IUserService userService)
        {
            _borrowService = borrowService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Borrow>> Get(int id)
        {

            try
            {
                var borrow = await _borrowService.GetById(id);
                if (_userService.UserRole.IsAdmin || _userService.UserRole.IsFinance ||
                        (_userService.UserRole.CanViewBorrow && _userService.Employee.EmployeeSiteId == borrow.SiteId) ||
                        _userService.Employee.EmployeeId == borrow.RequestedById)
                    return Ok(borrow);

                return Forbid();
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


        [HttpPost]
        public async Task<ActionResult<List<Borrow>>> GetByCondition(GetBorrowsDTO getBorrowsDTO)
        {
            var Borrows = await _borrowService.GetByCondition(getBorrowsDTO);

            return Ok(Borrows);
        }

        [HttpPost("request")]
        public async Task<ActionResult<int>> Post(CreateBorrowDTO borrowDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanRequestBorrow)
                return Forbid();

            try
            {
                var borrow = await _borrowService.RequestBorrow(borrowDTO);
                return Ok(borrow.BorrowId);
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

        [HttpPost("approve")]
        public async Task<ActionResult<Borrow>> Approve(ApproveBorrowDTO approveDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanApproveBorrow)
                return Forbid();

            try
            {
                var borrow = await _borrowService.ApproveBorrow(approveDTO);
                return Ok(borrow);
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
        public async Task<ActionResult<Borrow>> Decline(DeclineBorrowDTO declineDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanApproveBorrow)
                return Forbid();

            try
            {
                var borrow = await _borrowService.DeclineBorrow(declineDTO);
                return Ok(borrow);
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
        public async Task<ActionResult<Borrow>> Hand(HandBorrowDTO handDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanHandBorrow)
                return Forbid();

            try
            {
                var borrow = await _borrowService.HandBorrow(handDTO);
                return Ok(borrow);
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
