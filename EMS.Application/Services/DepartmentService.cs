using EMS_Backend_Project.EMS.Application.DTOs.DepartmentDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.DepartmentManagement;

namespace EMS_Backend_Project.EMS.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public Task AddDepartmentAsync(string name)
        {
            return _departmentRepository.AddDepartmentQuery(name);
        }

        public Task DeleteDepartmentAsync(int deptId)
        {
            return _departmentRepository.DeleteDepartmentQuery(deptId);
        }

        public async Task<ICollection<GetDepartmentDTO>> GetAllDepartmentAsync()
        {
            return await _departmentRepository.GetAllDepartmentQuery();
        }

        public async Task<GetDepartmentDTO> GetDepartmentByIdAsync(int deptId)
        {
            return await _departmentRepository.GetDepartmentByIdQuery(deptId);
        }

        public Task UpdateDepartmentAsync(int deptId, string name)
        {
            return _departmentRepository.UpdateDepartmentQuery(deptId, name);
        }
    }
}