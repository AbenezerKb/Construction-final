using ERP.Context;
using ERP.Models;
using ERP.Services.FileServices;
using ERP.Services.SiteServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Employee")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("{fileName}")]
        public async Task<FileContentResult> Get(string fileName)
        {
            var file = await _fileService.GetFile(fileName, this);

            return file;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<string>> Upload(IFormFile file)
        {
            var name = await _fileService.SaveFile(file);
            return Ok(name);
        }

        [HttpGet("download/{fileName}")]
        public async Task<FileContentResult> Download(string fileName)
        {
            var file =await  _fileService.DownloadFile(fileName, this);

            return file;
        }



        
    }
}
