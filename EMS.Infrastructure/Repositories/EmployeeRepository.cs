using AutoMapper;
using EMS_Backend_Project.EMS.Application.DTOs.EmployeeDTOs;
using EMS_Backend_Project.EMS.Application.DTOs.UserDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.EmployeeDashboard;
using EMS_Backend_Project.EMS.Common.CustomExceptions;
using EMS_Backend_Project.EMS.Domain.Entities;
using EMS_Backend_Project.EMS.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMS_Backend_Project.EMS.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<User>, IEmployeeRepository 
    {
        private readonly IMapper _mapper;
        public EmployeeRepository(ApplicationDBContext context, IMapper mapper) : base(context){
            _mapper = mapper;
        }

        public async Task<GetEmployeeDataDTO> GetProfileDataQuery(int id)
        {
            var employeeData = await _context.Users.Include(s => s.Employee)
                                                        .ThenInclude(d => d.Department)
                                                   .Where(c => c.UserId == id)
                                                   .Select(s => new GetEmployeeDataDTO
                                                   {
                                                       FirstName = s.FirstName,
                                                       LastName = s.LastName,
                                                       Email = s.Email,
                                                       PhoneNo = s.PhoneNo,
                                                       DateOfBirth = s.Employee.DateOfBirth,
                                                       Address = s.Employee.Address,
                                                       DepartmentName = s.Employee.Department.DepartmentName,
                                                       TeckStack = s.Employee.TechStack,
                                                       JoinDate = s.Employee.JoinDate
                                                   }).FirstOrDefaultAsync();

            if (employeeData == null)
                throw new DataNotFoundException<int>(id);

            return employeeData;
        }

        public async Task UpdateProfileQuery(int id, EmployeeUpdateDTO employeeUpdate)
        {
            // Include Employee when fetching User
            var existingUser = await _context.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (existingUser == null)
                throw new DataNotFoundException<string>($"No Employee found with id {id}");

            // Map the DTO to the existing entities (this preserves unchanged values)
            _mapper.Map(employeeUpdate, existingUser);
            if (existingUser.Employee != null)
            {
                _mapper.Map(employeeUpdate, existingUser.Employee);
            }

            // Set audit fields
            existingUser.UpdatedAt = DateTime.UtcNow;

            // If any properties needs special handling
            existingUser.PhoneNo = employeeUpdate.PhoneNo;
            if (existingUser.Employee != null)
            {
                existingUser.Employee.TechStack = employeeUpdate.TechStack;
            }
            // Since we're working with existing entities, we don't need separate Update calls
            await _context.SaveChangesAsync();
        }

        public async Task ChangePasswordQuery(int id, EmployeePwdUpdateDTO employeePwdUpdate)
        {
            var employee = await _context.Users.FindAsync(id);

            if (employee == null)
                throw new DataNotFoundException<string>($"No Employee found with Id {id}.");


            // Hash the password
            var passwordHasher = new PasswordHasher<EmployeePwdUpdateDTO>();
            var hashedPassword = passwordHasher.HashPassword(employeePwdUpdate, employeePwdUpdate.ConfirmPassword);

            employee.Password = hashedPassword;

            _context.Users.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
