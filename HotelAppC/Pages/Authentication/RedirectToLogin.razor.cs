using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Runtime.CompilerServices;

namespace HotelAppC.Pages.Authentication
{
    public partial class RedirectToLogin
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationState { get; set; }
        public bool NotAuthorized { get; set; } = false;

        protected async override Task OnInitializedAsync()
        {
            var returnUrl = Navigation.ToBaseRelativePath(Navigation.Uri);
            if (string.IsNullOrEmpty(returnUrl))
            {
                Navigation.NavigateTo("login", true);
            }
            else
            {
                Navigation.NavigateTo($"login?returnUrl={returnUrl}");
            }
        }
    }
}
