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

        public Task DeleteDepartmentAsync(int id)
        {
            return _departmentRepository.DeleteDepartmentQuery(id);
        }

        public async Task<ICollection<GetDepartmentDTO>> GetAllDepartmentAsync()
        {
            return await _departmentRepository.GetAllDepartmentQuery();
        }

        public async Task<GetDepartmentDTO> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetDepartmentByIdQuery(id);
        }

        public Task UpdateDepartmentAsync(int id, string name)
        {
            return _departmentRepository.UpdateDepartmentQuery(id, name);
        }
    }
}