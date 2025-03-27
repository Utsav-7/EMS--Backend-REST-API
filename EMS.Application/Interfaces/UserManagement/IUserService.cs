using EMS_Backend_Project.EMS.Application.DTOs.UserDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.UserManagement
{
    public interface IUserService
    {
        Task<ICollection<UserDTO>> GetAllUserAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task AddAdminAsync(AdminUserDTO adminUser);
        Task AddEmployeeAsync(EmplyeeUserDTO emplyeeUser);
        Task UpdateAdminByIdAsync(int id, AdminUserDTO adminUser);
        Task UpdateEmployeeByIdAsync(int id, EmplyeeUserDTO emplyeeUser);
        Task DeleteUserByIdAsync(int userId);
    }
}