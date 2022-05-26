using ERP.Context;
using ERP.Models;
using ERP.Services.EquipmentCategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class EquipmentCategoryController : Controller
    {
        private readonly DataContext context;
        private readonly IEquipmentCategoryService _equipmentCategoryService;

        public EquipmentCategoryController(DataContext context, IEquipmentCategoryService equipmentCategoryService)
        {
            this.context = context;
            _equipmentCategoryService = equipmentCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EquipmentCategory>>> GetAll()
        {
            var categories = await _equipmentCategoryService.GetAll();
            
            return Ok(categories);
        }

        [HttpGet("{EquipmentCategoryId}")]
        public async Task<ActionResult<List<EquipmentCategory>>> GetOne(int EquipmentCategoryId)
        {
            var category = await _equipmentCategoryService.GetById(EquipmentCategoryId);
            
            return Ok(category);
        }
    }
}
