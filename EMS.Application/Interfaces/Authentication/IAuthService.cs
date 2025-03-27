using EMS_Backend_Project.EMS.Domain.Entities;

namespace EMS_Backend_Project.EMS.Application.Interfaces.Authentication
{
    public interface IAuthService
    {
        Task<User> GetUserByEmailAsync(string email);
        string GenerateResetToken();
        Task<bool> UpdatePasswordDB(string newPassword, User user);
    }
}