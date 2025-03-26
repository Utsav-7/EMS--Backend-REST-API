using EMS_Backend_Project.EMS.Application.DTOs.DepartmentDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.DepartmentManagement
{
    public interface IDepartmentRepository
    {
        Task<ICollection<GetDepartmentDTO>> GetAllDepartmentQuery();
        Task<GetDepartmentDTO> GetDepartmentByIdQuery(int id);
        Task UpdateDepartmentQuery(int id, string name);
        Task AddDepartmentQuery(string name);
        Task DeleteDepartmentQuery(int id);
    }
}
