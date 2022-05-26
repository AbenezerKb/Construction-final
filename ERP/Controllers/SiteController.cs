using ERP.Context;
using ERP.Models;
using ERP.Services.SiteServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : Controller
    {
        private readonly DataContext context;
        private readonly ISiteService _siteService;

        public SiteController(DataContext context, ISiteService siteService)
        {
            this.context = context;
            _siteService = siteService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Site>> GetOne(int id)
        {
            try
            {
                Site site = await _siteService.GetOne(id);
                return Ok(site);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Site>>> Get()
        {
            var sites = await _siteService.GetAll();
            return Ok(sites);
        }

        // [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<ActionResult<Site>> AddSite(Site request)
        {
            var site = await _siteService.AddSite(request);

            return Ok(site);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("edit")]
        public async Task<ActionResult<Site>> EditSite(Site request)
        {
            try
            {
                var site = await _siteService.EditSite(request);
                return Ok(site);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
