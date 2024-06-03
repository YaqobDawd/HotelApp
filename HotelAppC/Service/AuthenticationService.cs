using Blazored.LocalStorage;
using Common;
using DTOS.ProjectIdentity;
using HotelAppC.Service.IService;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HotelAppC.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider auth;

        public AuthenticationService(HttpClient httpClient,ILocalStorageService LocalStorage , AuthenticationStateProvider auth)
        {
            this.httpClient = httpClient;
            localStorage = LocalStorage;
            this.auth = auth;
        }

        public async Task<bool> ConfirmEmail(string email, string token)
        {
            var response = await httpClient.GetAsync($"api/auth/EmailConfirmation?Email={email}&Token={token}");
            if(response.IsSuccessStatusCode)return true;
            else return false;
        }

        public async Task<AuthenticationResponseDTO> Login(UserForLoginDTO user)
        {
            var content = JsonConvert.SerializeObject(user);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/auth/signin", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthenticationResponseDTO>(contentTemp);
            if (response.IsSuccessStatusCode)
            {
                await localStorage.SetItemAsync(SD.Local_Token, result.Token);
                await localStorage.SetItemAsync(SD.Local_UserDetails, result.userForRetrunDTO);
                //bo logoute y da page load bbit
                ((AuthStateProvider)auth).NotifyUserLoggedIn(result.Token);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                return new AuthenticationResponseDTO { IsAuthSuccess = true };
            }
            else
            {
                return result;
            }
        }

        public async Task LogOut()
        {
            await localStorage.RemoveItemAsync(SD.Local_Token);
            await localStorage.RemoveItemAsync(SD.Local_UserDetails);
            httpClient.DefaultRequestHeaders.Authorization = null;
            ((AuthStateProvider)auth).NotifyUserLogout();
        }

        public async Task<RegistrationResponseDTO> Register(UserForRegisterDTO user)
        {
            var content = JsonConvert.SerializeObject(user);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/auth/signup", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RegistrationResponseDTO>(contentTemp);
            if (response.IsSuccessStatusCode)
            {
                return new RegistrationResponseDTO { IsRegistrationSuccess = true };
            }
            else
            {
                return result;
            }
        }

       
        public async Task<bool> ForgotPassword(ForgotPasswordDTO password)
        {
            var content = JsonConvert.SerializeObject(password);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/auth/forgotpassword", bodyContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ResetPassword(ResetPasswordDTO password)
        {
            var content=JsonConvert.SerializeObject(password);
            var bodyContent=new StringContent(content, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/auth/resetpassword", bodyContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
