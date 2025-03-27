using System.Linq;
using EMS_Backend_Project.EMS.Application.DTOs.DepartmentDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.DepartmentManagement;
using EMS_Backend_Project.EMS.Common.CustomExceptions;
using EMS_Backend_Project.EMS.Domain.Entities;
using EMS_Backend_Project.EMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EMS_Backend_Project.EMS.Infrastructure.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDBContext context) : base(context) {}

        public async Task<ICollection<GetDepartmentDTO>> GetAllDepartmentQuery()
        {
            // return department list with their total employee count
            var departmentList = await _context.Departments
                                                .GroupJoin(
                                                    _context.Employees, // Join with Employees
                                                    d => d.DepartmentId, // Key from Departments
                                                    e => e.DepartmentId, // Key from Employees
                                                    (department, employees) => new GetDepartmentDTO
                                                    {
                                                        DepartmentId = department.DepartmentId,
                                                        DepartmentName = department.DepartmentName,
                                                        TotalEmployee = employees.Count() 
                                                    }
                                                ).ToListAsync();

            if (departmentList == null)
                throw new DataNotFoundException<string>("Department Records not found.");

            return departmentList;
        }

        public async Task<GetDepartmentDTO> GetDepartmentByIdQuery(int deptId)
        {
            // get department by id with total employee count in that department
            var department = await _context.Departments
                                    .Where(c => c.DepartmentId == deptId)
                                    .GroupJoin(
                                        _context.Employees, // Join with Employees
                                        d => d.DepartmentId, // Key from Departments
                                        e => e.DepartmentId, // Key from Employees
                                        (department, employees) => new GetDepartmentDTO
                                        {
                                            DepartmentId = department.DepartmentId,
                                            DepartmentName = department.DepartmentName,
                                            TotalEmployee = employees.Count()
                                        }
                                    ).FirstOrDefaultAsync();
            if (department == null)
                throw new DataNotFoundException<int>(deptId);

            return department;
        }

        public async Task AddDepartmentQuery(string name)
        {
            // check department already exists or not
            var existingDepartment = await _context.Departments.FirstOrDefaultAsync(s => s.DepartmentName == name);

            if (existingDepartment != null)
                throw new AlreadyExistsException<string>(name);

            var newDepartment = new Department
            {
                DepartmentName = name,
                CreatedAt = DateTime.UtcNow
            };

            _context.Departments.Add(newDepartment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepartmentQuery(int deptId, string name)
        {
           // check department in db that you need to update
            var existingDepartment = await _context.Departments.FindAsync(deptId);

            if (existingDepartment == null)
                throw new DataNotFoundException<int>(deptId);

            existingDepartment.DepartmentName = name;
            existingDepartment.UpdatedAt = DateTime.UtcNow;

            _context.Departments.Update(existingDepartment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentQuery(int deptId)
        {
            // delete department by departmentID
            var existingDepartment = await _context.Departments.FindAsync(deptId);

            if (existingDepartment == null)
                throw new DataNotFoundException<int>(deptId);
            
            // remove hard delete
            _context.Departments.Remove(existingDepartment);
            await _context.SaveChangesAsync();
        }
    }
}