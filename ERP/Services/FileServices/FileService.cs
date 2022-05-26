using Microsoft.AspNetCore.Mvc;
using MimeTypes;

namespace ERP.Services.FileServices
{
    public class FileService : IFileService
    {
        private string _path;

        public FileService(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _path = Path.Combine(env.ContentRootPath, "UploadedFiles");
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            string name = $"file{DateTime.Now.ToString("_ffffff_yyyy_MM_dd_HH_mm_ss")}{Path.GetExtension(file.FileName)}";
            string savePath = Path.Combine(_path, name);

            using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                file.CopyTo(fileStream);

            return name;
        }

        public async Task<FileContentResult> GetFile(string fileName, ControllerBase controller)
        {
            string filePath = Path.Combine(_path, fileName);
            var file = File.ReadAllBytes(filePath);

            return controller.File(file, MimeTypeMap.GetMimeType(Path.GetExtension(fileName)));
        }

        public async Task<FileContentResult> DownloadFile(string fileName, ControllerBase controller)
        {
            string filePath = Path.Combine(_path, fileName);
            var file = File.ReadAllBytes(filePath);

            return controller.File(file, MimeTypeMap.GetMimeType(Path.GetExtension(fileName)), fileName);
        }
    }
}
