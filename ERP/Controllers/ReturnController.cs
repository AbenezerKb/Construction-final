using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.ReturnServices;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class ReturnController : Controller
    {
        private readonly IReturnService _returnService;
        private readonly IUserService _userService;

        public ReturnController(IReturnService returnService, IUserService userService)
        {
            _returnService = returnService;

            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Return>> Get(int id)
        {

            try
            {
                var returnResult = await _returnService.GetById(id);

                if (_userService.UserRole.IsAdmin || _userService.UserRole.IsFinance ||
                    (_userService.UserRole.CanViewBorrow && _userService.Employee.EmployeeSiteId == returnResult.SiteId) ||
                    (_userService.UserRole.CanViewBorrow && _userService.Employee.EmployeeSiteId == returnResult.ReturnEquipmentAssets.FirstOrDefault()?.Borrow.SiteId) ||
                    _userService.Employee.EmployeeId == returnResult.ReturnEquipmentAssets.FirstOrDefault()?.Borrow.RequestedById)
                    return Ok(returnResult);

                return Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Borrow>>> Get()
        {

            var returns = await _returnService.GetByCondition();

            return Ok(returns);
        }


        [HttpPost("return")]
        public async Task<ActionResult<int>> ReturnEquipments(ReturnBorrowDTO returnDTO)
        {
            if (!_userService.UserRole.IsAdmin && !_userService.UserRole.CanReturnBorrow)
                return Forbid();

            try
            {
                var borrowReturn = await _returnService.ReturnEquipments(returnDTO);
                return Ok(borrowReturn.ReturnId);
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

        [HttpPost("assets")]
        public async Task<ActionResult<List<EquipmentAsset>>> GetReturnableItems(GetReturnItemsDTO returnItemsDTO)
        {
            try
            {
                var assets = await _returnService.GetReturnableItems(returnItemsDTO);
                return Ok(assets);
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

        [HttpGet("damage")]
        public async Task<ActionResult<List<AssetDamage>>> GetDamageTypes()
        {

            var damages = await _returnService.GetDamageTypes();
            return Ok(damages);

        }

    }
}
