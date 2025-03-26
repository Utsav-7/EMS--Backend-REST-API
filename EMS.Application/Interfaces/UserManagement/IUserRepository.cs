using EMS_Backend_Project.EMS.Application.DTOs.UserDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.UserManagement
{
    public interface IUserRepository
    {
        Task<ICollection<UserDTO>> GetAllUserQuery();
        Task<UserDTO> GetUserByIdQuery(int id);
        Task AddEmployeeQuery(EmplyeeUserDTO emplyeeUserDTO);
        Task AddAdminQuery(AdminUserDTO adminUserDTO);
        Task UpdateAdminByIdQuery(int id, AdminUserDTO adminUserDTO);
        Task UpdateEmployeeByIdQuery(int id, EmplyeeUserDTO emplyeeUserDTO);
        Task DeleteUserByIdQuery(int id);
    }
}