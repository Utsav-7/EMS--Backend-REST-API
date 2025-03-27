using EMS_Backend_Project.EMS.Application.DTOs.EmployeeDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.EmployeeDashboard;

namespace EMS_Backend_Project.EMS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Task ChangePasswordAsync(int id, EmployeePwdUpdateDTO employeePwdUpdate)
        {
            return _employeeRepository.ChangePasswordQuery(id, employeePwdUpdate);
        }

        public async Task<GetEmployeeDataDTO> GetProfileDataAsync(int id)
        {
            return await _employeeRepository.GetProfileDataQuery(id);
        }

        public Task UpdateProfileAsync(int id, EmployeeUpdateDTO employeeUpdate)
        {
            return _employeeRepository.UpdateProfileQuery(id, employeeUpdate);
        }
    }
}