﻿using System.Linq;
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

        public async Task<GetDepartmentDTO> GetDepartmentByIdQuery(int id)
        {

            var department = await _context.Departments
                                    .Where(c => c.DepartmentId == id)
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
                throw new DataNotFoundException<int>(id);

            return department;
        }

        public async Task AddDepartmentQuery(string name)
        {
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

        public async Task UpdateDepartmentQuery(int id, string name)
        {
            var existingDepartment = await _context.Departments.FindAsync(id);

            if (existingDepartment == null)
                throw new DataNotFoundException<int>(id);

            existingDepartment.DepartmentName = name;
            existingDepartment.UpdatedAt = DateTime.UtcNow;

            _context.Departments.Update(existingDepartment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentQuery(int id)
        {
            var existingDepartment = await _context.Departments.FindAsync(id);

            if (existingDepartment == null)
                throw new DataNotFoundException<int>(id);

            _context.Departments.Remove(existingDepartment);
            await _context.SaveChangesAsync();
        }
    }
}