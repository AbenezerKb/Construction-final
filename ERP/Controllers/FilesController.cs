
using ERP.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers.FilesController
{
    [Route("api/[controller]")]
    public class FilesController: ControllerBase
    {

        private readonly IFileRepo _fileRepo;
        
        public FilesController(IFileRepo contractRepo)
        {
            Console.WriteLine("file constructor started");
            _fileRepo = contractRepo;
            Console.WriteLine("repo assigned constructor started");

        }     



        [HttpGet("id", Name = "GetFile")]
        public async Task<FileContentResult> GetFile(string fileName)
        {
            FileContentResult file = await _fileRepo.GetFile(fileName, this);
            Console.WriteLine("file created: "+file);
            return (file);

        }




        [HttpPost]
        public async Task<ActionResult<string>> SaveFile()
        {
            //IFormFile file


             IFormFile file = HttpContext.Request.Form.Files[0];
            //Files.FirstOrDefault();
            Console.WriteLine("file saving started inside controller");
             var name = await _fileRepo.SaveFile(file);

             return Ok(name);
             /*

            if (file.Length <= 0)
                return BadRequest("Empty file");

            //Strip out any path specifiers (ex: /../)
            var originalFileName = Path.GetFileName(file.FileName);

            //Create a unique file path
            var uniqueFileName = Path.GetRandomFileName();
            var uniqueFilePath = Path.Combine(@"C:\temp\", uniqueFileName);

            //Save the file to disk
            using (var stream = System.IO.File.Create(uniqueFilePath))
            {
                await file.CopyToAsync(stream);
            }

            return Ok($"Saved file {originalFileName} with size {file.Length / 1024m:#.00} KB using unique name {uniqueFileName}");


            */



        }


    }
}
