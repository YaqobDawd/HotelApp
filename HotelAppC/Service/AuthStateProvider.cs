using Blazored.LocalStorage;
using Common;
using HotelAppC.Helper;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace HotelAppC.Service
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public AuthStateProvider(HttpClient httpClient,ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage.GetItemAsync<string>(SD.Local_Token);
            if (token == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
                httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("bearer", token);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.GetClaimsFromJwt(token),"jwtAuthType")));
            
        }


        public void NotifyUserLoggedIn(string Token)
        {
            var AutheticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.GetClaimsFromJwt(Token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(AutheticatedUser));

            NotifyAuthenticationStateChanged(authState);

        }

        public void NotifyUserLogout()
        {
            var AuthState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(AuthState);
        }
    }
}
