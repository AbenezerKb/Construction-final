using ERP.Context;
using ERP.DTOs.Item;
using ERP.Models;
using ERP.Services.DamageServices;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class DamageController : Controller
    {
        private readonly IDamageService _damageService;
        private readonly IUserService _userService;

        public DamageController(IDamageService damageService, IUserService userService)
        {
            _damageService = damageService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AssetDamage>>> Get()
        {
            if (!_userService.UserRole.IsAdmin) Forbid();

            var damages = await _damageService.GetDamages();

            return Ok(damages);
        }

        [HttpPost("add")]
        public async Task<ActionResult<AssetDamage>> Add(AddDamageDTO damageDTO)
        {

            if (!_userService.UserRole.IsAdmin) Forbid();

            var damage = await _damageService.AddDamage(damageDTO);

            return Ok(damage);
        }

        [HttpPost("update")]
        public async Task<ActionResult<bool>> Update(List<AssetDamage> damages)
        {

            if (!_userService.UserRole.IsAdmin) Forbid();

            var damage = await _damageService.UpdateDamages(damages);

            return Ok(damage);
        }
    }
}
