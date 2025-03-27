using EMS_Backend_Project.EMS.Application.DTOs.DepartmentDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.DepartmentManagement
{
    public interface IDepartmentService
    {
        Task<ICollection<GetDepartmentDTO>> GetAllDepartmentAsync();
        Task<GetDepartmentDTO> GetDepartmentByIdAsync(int deptId);
        Task UpdateDepartmentAsync(int deptId, string name);
        Task AddDepartmentAsync(string name);
        Task DeleteDepartmentAsync(int deptId);
    }
}