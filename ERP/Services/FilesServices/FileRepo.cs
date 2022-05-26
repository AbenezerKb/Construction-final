using Microsoft.AspNetCore.Mvc;
using MimeTypes;
namespace ERP.Services
{
    public class FileRepo: IFileRepo
    {
        
        private string _path;
        public FileRepo(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _path = Path.Combine(env.ContentRootPath, "UploadedFiles");
        }
        public async Task<string> SaveFile(IFormFile file)
        {
            string filename = Guid.NewGuid().ToString();


            if (file.Length < 0)
            {
                throw new ArgumentNullException();
            }

            Console.WriteLine("the file fileName: " + file.FileName);
            string name = $"file{filename}{Path.GetExtension(file.FileName)}";
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


    }
}
