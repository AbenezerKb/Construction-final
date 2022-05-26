using ERP.Context;
using ERP.Models;
using ERP.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.DTOs.Item;
using ERP.Services.ItemServices;
using Microsoft.AspNetCore.Authorization;
using ERP.Services.User;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class ItemController : Controller
    {
        private readonly DataContext context;
        private readonly IItemService _itemService;
        private readonly IUserService _userService;

        public ItemController(DataContext context, IItemService itemService, IUserService userService)
        {
            this.context = context;
            _itemService = itemService;
            _userService = userService;
        }

        [HttpPost("import-asset")]
        public async Task<ActionResult<int>> ImportAssetNumbers(ImportAssetNoDTO importDTO)
        {
            if (!_userService.UserRole.IsAdmin)
                return Forbid();

            var result = await _itemService.ImportAssetNumbers(importDTO);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetAll()
        {
            var items = await  _itemService.GetAll();

            return Ok(items);
        }

        [HttpGet("asset")]
        public async Task<ActionResult<List<EquipmentAsset>>> GetAssets(int modelId)
        {
            var items = await _itemService.GetAssets(modelId);

            return Ok(items);
        }

        [HttpGet("asset-clean")]
        public async Task<ActionResult<List<EquipmentAsset>>> GetCleanAssets(int modelId)
        {
            var items = await _itemService.GetCleanAssets(modelId);

            return Ok(items);
        }

        [HttpPost("minimum-stock")]
        public async Task<ActionResult<List<MinimumStockItemDTO>>> GetMinimumStock(GetMinimumStockItemsDTO minimumStockDTO)
        {
            if (!_userService.UserRole.IsAdmin || !_userService.UserRole.CanGetStockNotification)
                return Forbid();


            var items = await _itemService.GetMinimumStockItems(minimumStockDTO);

            return Ok(items);
        }

        [HttpPost("minimum-stock/update")]
        public async Task<ActionResult<bool>> UpdateMinimumStock(List<MinimumStockItemDTO> minimumStockItems)
        {
            var result = await _itemService.UpdateMinimumStockItems(minimumStockItems);

            return Ok(result);
        }

        [HttpGet("{ItemId}")]
        public async Task<ActionResult<Item>> GetOne(int ItemId)
        {
           
            try
            {
                var item = await _itemService.GetById(ItemId);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("equipmentmodel/{ModelId}")]
        public async Task<ActionResult<EquipmentModel>> GetModel(int ModelId)
        {

            try
            {
                var model = await _itemService.GetModel(ModelId);
                return Ok(model);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("equipmentmodels")]
        public async Task<ActionResult<List<EquipmentModel>>> GetEquipmentModels()
        {
            var models = await _itemService.GetEquipmentModels();
                
            return Ok(models);

        }

        [HttpGet("material")]
        public async Task<ActionResult<List<Item>>> GetMaterial()
        {
            var items = await _itemService.GetMaterials();

            return Ok(items);
        }

        [HttpGet("material/transferable")]
        public async Task<ActionResult<List<Item>>> GetTransferableMaterial()
        {
            var items = await _itemService.GetTransferableMaterials();

            return Ok(items);
        }

        [HttpGet("equipment")]
        public async Task<ActionResult<List<Item>>> GetEquipment()
        {
            var items = await _itemService.GetEquipments();

            return Ok(items);
        }

        [HttpGet("equipmentcategory")]
        public async Task<ActionResult<List<EquipmentCategory>>> GetCategory()
        {
            var categories = await _itemService.GetEquipmentCategories();

            return Ok(categories);
        }

        [HttpPost("material/add")]
        public async Task<ActionResult<List<Item>>> CreateMaterialItem(CreateMaterialItemDTO materialItemDTO)
        {
            var item = await _itemService.CreateMaterialItem(materialItemDTO);

            return Ok(item);
        }

        [HttpPost("equipment/add")]
        public async Task<ActionResult<List<Item>>> CreateEquipmentItem(CreateEquipmentItemDTO equipmentItemDTO)
        {
            var item = await _itemService.CreateEquipmentItem(equipmentItemDTO);

            return Ok(item);
        }

        [HttpPost("equipmentcategory/add")]
        public async Task<ActionResult<List<EquipmentCategory>>> CreateEquipmentCategory(CreateEquipmentCategoryDTO equipmentCategoryDTO)
        {
            var equipmentCategory = await _itemService.CreateEquipmentCategory(equipmentCategoryDTO);

            return Ok(equipmentCategory);
        }

        [HttpPost("equipmentmodel/add")]
        public async Task<ActionResult<List<EquipmentModel>>> CreateEquipmentModel(CreateEquipmentModelDTO equipmentModelDTO)
        {
            var equipmentModel = await _itemService.CreateEquipmentModel(equipmentModelDTO);

            return Ok(equipmentModel);
        }

        [HttpPost("material/edit")]
        public async Task<ActionResult<List<EquipmentModel>>> EditMaterial(EditMaterialItemDTO materialItemDTO)
        {
            try
            {
                var item = await _itemService.EditMaterial(materialItemDTO);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("equipment/edit")]
        public async Task<ActionResult<List<EquipmentModel>>> EditEquipmentl(EditEquipmentItemDTO equipmentItemDTO)
        {
            try
            {
                var item = await _itemService.EditEquipment(equipmentItemDTO);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("equipmentmodel/edit")]
        public async Task<ActionResult<List<EquipmentModel>>> EditEquipmentModel(EditEquipmentModelDTO equipmentModelDTO)
        {
            try
            {
                var equipmentModel = await _itemService.EditEquipmentModel(equipmentModelDTO);
                return Ok(equipmentModel);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("qty")]
        public async Task<ActionResult<int>> GetItemSiteQty(GetQtyDTO getQtyDTO)
        {
            var qty = await _itemService.GetItemQty(getQtyDTO);

            return Ok(qty);
        }
    }
}
