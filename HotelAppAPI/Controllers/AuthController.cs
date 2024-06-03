using Common;
using DataAccess.Model;
using DTOS.ProjectIdentity;
using HotelAppAPI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;

namespace HotelAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;
        private readonly APISetting aPISetting;

        public AuthController(SignInManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<APISetting> options, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
            this.aPISetting = options.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserForRegisterDTO userForRegisterDTO)
        {
            if (userForRegisterDTO == null || !ModelState.IsValid) return BadRequest();
            var user = new ApplicationUser
            {
                Name = userForRegisterDTO.Email,
                UserName = userForRegisterDTO.Email,
                Email = userForRegisterDTO.Email,
                PhoneNumber = userForRegisterDTO.PhoneNo,
            };
            var result = await userManager.UserManager.CreateAsync(user, userForRegisterDTO.Password);
            if (!result.Succeeded)
            {
                var Error = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDTO
                {
                    Erorrs = Error,
                    IsRegistrationSuccess = false
                });

            }
            //bo confirm krna email de ve code danin
            var token = await userManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            var param = new Dictionary<string, string>
                        {
                            {"token", codeEncoded },
                            {"email", user.Email }
                        };
            var callback = QueryHelpers.AddQueryString(userForRegisterDTO.ClientURI, param);
            await emailSender.SendEmailAsync(user.Email, "Email Confirmation - Hotel App",
                $"<h3 style='color:purple'>Verify your email address so we know it’s really you.</h3>" +
                $"<a style='text-decoration:none;padding:10px;font-size:18px;background-color:purple;color:#ffffff;border-radius:5px;'" +
                $" href='{callback}'>Verify Email Address</a>");

            var roleResult = await userManager.UserManager.AddToRoleAsync(user, SD.Role_Cutstomer);
            if (!roleResult.Succeeded)
            {
                var Error = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDTO
                {
                    Erorrs = Error,
                    IsRegistrationSuccess = false
                });
            }
            return StatusCode(201);

        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(UserForLoginDTO userForLoginDTO)
        {
            try
            {

                var result = await userManager.PasswordSignInAsync(userForLoginDTO.Name.ToLower(), userForLoginDTO.Password, false, false);

                if (result.Succeeded)
                {

                    var user = await userManager.UserManager.FindByNameAsync(userForLoginDTO.Name);

                    if (user == null)
                    {
                        return Unauthorized(new AuthenticationResponseDTO
                        {
                            IsAuthSuccess = false,
                            ErrorMessage = "You entered wrong user or password"
                        });
                    }

                    // everything is valid and we need to login the user

                    if (!await userManager.UserManager.IsEmailConfirmedAsync(user))
                    {
                        return Unauthorized(new AuthenticationResponseDTO
                        {
                            ErrorMessage = "Email is not confirmed check your inbox"
                        });
                    }

                    var signingCredentials = GetSigningCredentials();
                    var claims = await GetClaims(user);
                    var tokenOptions = new JwtSecurityToken(
                        issuer: aPISetting.ValidIssuer,
                        audience: aPISetting.ValidAudience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: signingCredentials
                        );
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    return Ok(new AuthenticationResponseDTO
                    {
                        IsAuthSuccess = true,
                        Token = token,
                        userForRetrunDTO = new UserForRetrunDTO
                        {
                            Id = user.Id,
                            Name = user.UserName,
                            Email = user.Email,
                            PhoneNo = user.PhoneNumber

                        }
                    });
                }
                else
                {
                    return Unauthorized(new AuthenticationResponseDTO { IsAuthSuccess = false, ErrorMessage = "یوزەر یان پاسسورد د خەلەتن" });
                }
            }
            catch (Exception)
            {

                throw;
            }



        }



        private SigningCredentials GetSigningCredentials()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(aPISetting.SecretKey));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser applicationUser)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, applicationUser.Name),
                new Claim(ClaimTypes.Email, applicationUser.Email),
                new Claim("Id",applicationUser.Id)
            };
            var roles = await userManager.UserManager.GetRolesAsync(await userManager.UserManager.FindByEmailAsync(applicationUser.Email));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;


        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> EmailConfirmation(string Email, string Token)
        {
            var user = await userManager.UserManager.FindByEmailAsync(Email);
            if (user == null)
                return BadRequest("Invalid Email Confirmation Request");

            var codeDecodedBytes = WebEncoders.Base64UrlDecode(Token);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
            var confirmResult = await userManager.UserManager.ConfirmEmailAsync(user, codeDecoded);
            if (!confirmResult.Succeeded)
                return BadRequest("Invalid Email Confirmation Request");

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await userManager.UserManager.FindByEmailAsync(forgotPasswordDTO.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var token = await userManager.UserManager.GeneratePasswordResetTokenAsync(user);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            var param = new Dictionary<string, string>
                            {
                                {"token", codeEncoded },
                                {"email", forgotPasswordDTO.Email }
                            };
            var callback = QueryHelpers.AddQueryString(forgotPasswordDTO.ClientURI, param);
            await emailSender.SendEmailAsync(user.Email, "Reset Password - Hotel App",
                $"<h3 style='color:purple'>You can reset your password now , just press reset button.</h3>" +
                $"<a style='text-decoration:none;padding:10px;font-size:18px;background-color:purple;color:#ffffff;border-radius:5px;'" +
                $" href='{callback}'>Reset Your Password</a>");
            return Ok();
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await userManager.UserManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var codeDecodedBytes = WebEncoders.Base64UrlDecode(resetPasswordDTO.Token);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
            var resetPassResult = await userManager.UserManager.ResetPasswordAsync(user, codeDecoded, resetPasswordDTO.Password);
            if (!resetPassResult.Succeeded)
            {
                return BadRequest("Invalid Request");
            }
            return Ok();
        }
    }
}
