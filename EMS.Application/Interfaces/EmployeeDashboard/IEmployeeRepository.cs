using EMS_Backend_Project.EMS.Application.DTOs.EmployeeDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.EmployeeDashboard
{
    public interface IEmployeeRepository
    {
        Task<GetEmployeeDataDTO> GetProfileDataQuery(int id);
        Task UpdateProfileQuery(int id, EmployeeUpdateDTO employeeUpdate);
        Task ChangePasswordQuery(int id, EmployeePwdUpdateDTO employeePwdUpdate);
    }
}
