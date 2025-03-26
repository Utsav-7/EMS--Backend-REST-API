using EMS_Backend_Project.EMS.Application.DTOs.UserDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.UserManagement;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EMS_Backend_Project.EMS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task AddAdminAsync(AdminUserDTO adminUser)
        {
            return _userRepository.AddAdminQuery(adminUser);
        }

        public Task AddEmployeeAsync(EmplyeeUserDTO emplyeeUser)
        {
            return _userRepository.AddEmployeeQuery(emplyeeUser);
        }

        public Task DeleteUserByIdAsync(int id)
        {
            return _userRepository.DeleteUserByIdQuery(id);
        }

        public async Task<ICollection<UserDTO>> GetAllUserAsync()
        {
            return await _userRepository.GetAllUserQuery();
        }

        public Task<UserDTO> GetUserByIdAsync(int id)
        {
            return _userRepository.GetUserByIdQuery(id);
        }

        public Task UpdateAdminByIdAsync(int id, AdminUserDTO adminUser)
        {
            return _userRepository.UpdateAdminByIdQuery(id, adminUser);
        }

        public Task UpdateEmployeeByIdAsync(int id, EmplyeeUserDTO emplyeeUser)
        {
            return _userRepository.UpdateEmployeeByIdQuery(id, emplyeeUser);
        }
    }
}