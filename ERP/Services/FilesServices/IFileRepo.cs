using Microsoft.AspNetCore.Mvc;

namespace ERP.Services
{
    public interface IFileRepo
    {
       // Task<FileContentResult> GetFile(string fileName, ControllerBase controller);
        //Task<string> SaveFile(IFormFile formFile);
        Task<string> SaveFile(IFormFile file);
        //Task<FileContentResult> GetFile(string fileName, ControllerBase controller);
        Task<FileContentResult> GetFile(string fileName, ControllerBase controller);
    }
}
