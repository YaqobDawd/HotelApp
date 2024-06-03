using DTOS.ProjectIdentity;

namespace HotelAppC.Service.IService
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDTO> Register(UserForRegisterDTO user);
        Task<AuthenticationResponseDTO> Login(UserForLoginDTO user);
        Task LogOut();

        Task <bool> ConfirmEmail(string email,string token);
        Task<bool> ForgotPassword(ForgotPasswordDTO password);

        Task<bool> ResetPassword(ResetPasswordDTO password);
    }
}
