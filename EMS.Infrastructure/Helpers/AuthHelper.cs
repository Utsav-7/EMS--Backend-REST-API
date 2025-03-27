using EMS_Backend_Project.EMS.Application.Interfaces.Authentication;
using EMS_Backend_Project.EMS.Application.Interfaces;
using EMS_Backend_Project.EMS.Domain.Entities;
using EMS_Backend_Project.EMS.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using EMS_Backend_Project.EMS.Infrastructure.Repositories;
using EMS_Backend_Project.EMS.Common.CustomExceptions;

namespace EMS_Backend_Project.EMS.Infrastructure.Services
{
    public class AuthHelper : Repository<User>, IAuthService
    {
        private readonly IEmailService _emailService;

        public AuthHelper(IEmailService emailService, ApplicationDBContext context) : base(context)
        {
            _emailService = emailService;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            // find user by user mail
            var user = await _context.Users.FirstOrDefaultAsync(s => s.Email == email && s.IsDeleted == false);

            if (user == null || user.IsDeleted)
                throw new DataNotFoundException<string>($"No user found with {email}.");

            return user;
        }

        public string GenerateResetToken()
        {
            // Generate a reset token (Base64-encoded GUID)
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 20);

            if (token == null)
                throw new Exception("Token Not Generated.");

            string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return encodedToken;
        }

        public async Task<bool> UpdatePasswordDB(string newPassword, User user)
        {
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, newPassword);

            user.UpdatedAt = DateTime.UtcNow;
            
            _context.Users.Update(user);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}