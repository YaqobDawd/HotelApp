using Microsoft.AspNetCore.Components.Forms;

namespace HotelAppS.Services
{
    public interface IFileUpload
    {
        Task<string> UploadFile(IBrowserFile file);
        bool DeleteFile(string fileName);
    }
}
