using EMS_Backend_Project.EMS.Application.DTOs.Authentication;

namespace EMS_Backend_Project.EMS.Application.Interfaces.Authentication
{
    public interface IAuthRepository
    {
        Task<string> LoginAsync(UserLoginDTO userLogin);
        Task<string> ForgotPassword(ForgotPwdDTO forgotPwd);
        Task<string> ResetPassword(ResetPwdDTO resetPwd);
    }
}