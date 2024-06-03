using Microsoft.JSInterop;
using Newtonsoft.Json.Converters;
using System.Runtime.CompilerServices;

namespace HotelAppC.Helper
{
    public static class IJSRuntimeExtension
    {
        public static async Task ToastrSuccess(this IJSRuntime jSRuntime, string message)
        {
            await jSRuntime.InvokeVoidAsync("ShowToastr","success", message);
        }

        public static async Task ToastrError(this IJSRuntime jSRuntime, string message)
        {
            await jSRuntime.InvokeVoidAsync("ShowToastr", "error", message);
        }
        public static async Task StartCarousel(this IJSRuntime jSRuntime )
        {
            
        }
        public static async Task<bool> ShowConfirm(this IJSRuntime jSRuntime)
        {
            return await jSRuntime.InvokeAsync<bool>("SwalConfirm");
        }

        

    }
}
