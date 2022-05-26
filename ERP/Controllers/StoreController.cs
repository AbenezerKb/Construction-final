using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : Controller
    {
        private readonly DataContext context;

        public StoreController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Store>>> Get()
        {
            var stores = await context.Stores.Include(store => store.Site).ToListAsync();

            return Ok(stores);
        }
    }
}
