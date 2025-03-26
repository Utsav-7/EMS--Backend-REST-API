using EMS_Backend_Project.EMS.Application.DTOs.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS_Backend_Project.EMS.Infrastructure.Services
{
    public class JWTTokenHelper
    {
        public readonly IConfiguration _configuration;
        public JWTTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Token generation
        internal string GenerateToken(UserLoginDTO login, int roleId, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Dictionary<int, string> roles = new Dictionary<int, string> { { 1, "Administrator" }, { 2, "Employee" } };
            string newRole = roles[roleId];

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, login.Email),
                new Claim(ClaimTypes.Role, newRole)
            };

            int tokenExpireTime = int.Parse(_configuration["Jwt:ExpireTimeInMinutes"] ?? "60");

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpireTime),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}