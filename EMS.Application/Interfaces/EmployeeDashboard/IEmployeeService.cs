using EMS_Backend_Project.EMS.Application.DTOs.EmployeeDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.EmployeeDashboard
{
    public interface IEmployeeService
    {
        Task<GetEmployeeDataDTO> GetProfileDataAsync(int id);
        Task UpdateProfileAsync(int id, EmployeeUpdateDTO employeeUpdate);
        Task ChangePasswordAsync(int id, EmployeePwdUpdateDTO employeePwdUpdate);
    }
}
