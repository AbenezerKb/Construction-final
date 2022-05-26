using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.MiscServices;
using ERP.Services.SiteServices;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiscController : Controller
    {
        private readonly IMiscService _miscService;
        private readonly IUserService _userService;

        public MiscController(IMiscService miscService, IUserService userService)
        {
            _miscService = miscService;
            _userService = userService;
        }

        [HttpGet("company")]
        public async Task<ActionResult<CompanyNamePrefixDTO>> GetNamePrefix()
        {
            var nameData = await _miscService.GetCompanyNamePrefix();

            return Ok(nameData);
        }

        [HttpPost("company")]
        public async Task<ActionResult<bool>> SetNamePrefix(CompanyNamePrefixDTO companyDTO)
        {
            if (!_userService.UserRole.IsAdmin)
                return Forbid();

            var result = await _miscService.SetCompanyNamePrefix(companyDTO);

            return Ok(result);
        }
    }
}
