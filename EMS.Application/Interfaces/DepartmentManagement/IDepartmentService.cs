using EMS_Backend_Project.EMS.Application.DTOs.DepartmentDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.DepartmentManagement
{
    public interface IDepartmentService
    {
        Task<ICollection<GetDepartmentDTO>> GetAllDepartmentAsync();
        Task<GetDepartmentDTO> GetDepartmentByIdAsync(int id);
        Task UpdateDepartmentAsync(int id, string name);
        Task AddDepartmentAsync(string name);
        Task DeleteDepartmentAsync(int id);
    }
}
