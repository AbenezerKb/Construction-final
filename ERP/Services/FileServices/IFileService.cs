using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.FileServices
{
    public interface IFileService
    {
        Task<FileContentResult> DownloadFile(string fileName, ControllerBase controller);
        Task<FileContentResult> GetFile(string fileName, ControllerBase controller);
        Task<string> SaveFile(IFormFile file);
    }
}
