using EMS_Backend_Project.EMS.Application.DTOs.Authentication;
using EMS_Backend_Project.EMS.Application.Interfaces;
using EMS_Backend_Project.EMS.Application.Interfaces.Authentication;
using EMS_Backend_Project.EMS.Common.CustomExceptions;
using EMS_Backend_Project.EMS.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS_Backend_Project.EMS.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IAuthService _authService;
        private readonly JWTTokenHelper _tokenService;
        private readonly IEmailService _emailService;
        private static ConcurrentDictionary<string, string> _resetTokens = new ConcurrentDictionary<string, string>();

        public AuthRepository(IAuthService authService, IEmailService emailService, JWTTokenHelper tokenService)
        {
            _authService = authService;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<string> LoginAsync(UserLoginDTO userLogin)
        {
            var user = await _authService.GetUserByEmailAsync(userLogin.Email);

            if (user.Active == false)
                throw new Exception("User is not Active.");

            if (user == null)
                throw new DataNotFoundException<string>("USER NOT FOUND");

            var passwordHasher = new PasswordHasher<UserLoginDTO>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(userLogin, user.Password, userLogin.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
                throw new Exception("Invalid email or password");

            string token = _tokenService.GenerateToken(userLogin, user.RoleId, user.UserId);
            Console.WriteLine($"Generated Token: {token}");

            return token;
        }

        public async Task<string> ForgotPassword(ForgotPwdDTO forgotPwd)
        {
            var user = await _authService.GetUserByEmailAsync(forgotPwd.Email);

            if (user == null)
                throw new DataNotFoundException<string>(forgotPwd.Email);

            var encodedToken = _authService.GenerateResetToken();
            Console.WriteLine($"Reset Token: {encodedToken}");

            string resetUrl = $"https://yourdomain.com/reset-password?email={Uri.EscapeDataString(user.Email)}&toke{encodedToken}";

            // Email body
            var emailBody = $@"
                        <h2>Password Reset Request</h2>
                        <p>Click <a href='{resetUrl}'>here</a> to reset your password.</p>
                        <p>If you did not request this, please ignore this email.</p>";


            await _emailService.SendEmailAsync(user.Email, "Reset Password", emailBody);

            return "Reset link sent to email.";
        }

        public async Task<string> ResetPassword(ResetPwdDTO resetPwd)
        {
            if (resetPwd.NewPassword != resetPwd.ConfirmPassword)
                throw new Exception("New Password and Confirm Password are not match.");

            string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPwd.Token));

            var user = await _authService.GetUserByEmailAsync(resetPwd.Email);

            string expectedToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 20);

            if (decodedToken.Length != expectedToken.Length)
                throw new Exception("Invalid reset token.");

            bool status = await _authService.UpdatePasswordDB(resetPwd.ConfirmPassword, user);

            _resetTokens.TryRemove(resetPwd.Email, out _);

            return status ? "Password has been reset successfully." : "Password has been not reset";
        }
    }
}