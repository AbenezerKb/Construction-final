using BrunoZell.ModelBinding;
using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.AssetNumberServices;
using ERP.Services.EquipmentAssetServices;
using ERP.Services.FileServices;
using ERP.Services.ItemSiteQtyServices;
using ERP.Services.ReportServices;
using ERP.Services.SiteServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AAController : Controller
    {
        private readonly IFileService _fileService;
        private readonly DataContext context;
        private readonly IEquipmentAssetService _equipmentAssetService;
        private readonly IItemSiteQtyService _itemSiteQtyService;
        private readonly IReportService _reportService;
        private readonly IAssetNumberService _assetNumberService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        private readonly ISiteService _siteService;

        public AAController(IFileService fileService, DataContext context, IEquipmentAssetService equipmentAssetService, IItemSiteQtyService itemSiteQtyService, IReportService reportService, IAssetNumberService assetNumberService, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _fileService = fileService;
            this.context = context;
            _equipmentAssetService = equipmentAssetService;
            _itemSiteQtyService = itemSiteQtyService;
            _reportService = reportService;
            _assetNumberService = assetNumberService;
            _env = env;
        }

        [HttpGet("a")]
        public async Task<FileContentResult> GetA(string fileName)
        {
            FileContentResult file = await _fileService.GetFile(fileName, this);
            
            return (file);

        }

        [HttpPost]
        public async Task<ActionResult<IFormFile>> Post([ModelBinder(BinderType = typeof(JsonModelBinder))] ReturnBorrowAsset value,
    IList<IFormFile> files)
        {
            var name = await _fileService.SaveFile(files[0]);          
            
            return Ok(name);
        }

        [HttpGet]
        public async Task<ActionResult<List<Site>>> Get(int itemId, int count)
        {

            return Ok(await _assetNumberService.GenerateAssetNumbers(itemId, count));
        }
    }
}
