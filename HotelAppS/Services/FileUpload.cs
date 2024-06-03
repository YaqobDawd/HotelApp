using Microsoft.AspNetCore.Components.Forms;

namespace HotelAppS.Services
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FileUpload(IWebHostEnvironment webHostEnvironment ,IHttpContextAccessor httpContextAccessor)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }
        public bool DeleteFile(string fileName)
        {
            try
            {
                var Filepath = Path.Combine(webHostEnvironment.WebRootPath, "RoomImages", fileName);
                if(File.Exists(Filepath))
                {
                    File.Delete(Filepath);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> UploadFile(IBrowserFile file)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(file.Name);
                var FileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                var fileDirectory = $"{webHostEnvironment.WebRootPath}\\RoomImages";
                var filePath = Path.Combine(fileDirectory, FileName);
                var memoryStream = new MemoryStream();
                await file.OpenReadStream(maxAllowedSize: 3*1024*1024).CopyToAsync(memoryStream);
                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }
                await using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fs);
                }
                var url = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}/";
                var fullPath = $"{url}RoomImages/{FileName}";
                return fullPath;
            }
            catch (Exception)
            {

                throw;
            }
            

        }
    }
}
