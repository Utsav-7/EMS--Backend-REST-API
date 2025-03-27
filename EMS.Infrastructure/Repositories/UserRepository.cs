using AutoMapper;
using EMS_Backend_Project.EMS.Application.DTOs.UserDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces;
using EMS_Backend_Project.EMS.Application.Interfaces.UserManagement;
using EMS_Backend_Project.EMS.Common.CustomExceptions;
using EMS_Backend_Project.EMS.Domain.Entities;
using EMS_Backend_Project.EMS.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMS_Backend_Project.EMS.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDBContext context, IEmailService emailService, IMapper mapper) : base(context)
        {
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<ICollection<UserDTO>> GetAllUserQuery()
        {
            // Get all the user which are not deleted
            var usersList = await _context.Users.Where(c => c.IsDeleted == false).Select(s => new UserDTO
            {
                UserId = s.UserId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNo = s.PhoneNo,
                RoleName = s.Role.RoleName,
                Active = s.Active,
                CreatedAt = s.CreatedAt
            }).ToListAsync();

            if (usersList == null)
                throw new DataNotFoundException<string>("No User found.");

            return usersList;
        }

        public async Task<UserDTO> GetUserByIdQuery(int id)
        {
            // get specific user by userId ensure that it is not deleted
            var user = await _context.Users.Where(c => c.UserId == id && c.IsDeleted == false).Select(s => new UserDTO
            {
                UserId = s.UserId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNo = s.PhoneNo,
                RoleName = s.Role.RoleName,
                Active = s.Active,
                CreatedAt = s.CreatedAt
            }).FirstOrDefaultAsync();

            if (user == null)
                throw new DataNotFoundException<int>(id);

            return user;
        }

        public async Task AddAdminQuery(AdminUserDTO adminUserDTO)
        {
            // check same admin exist or not by Email
            var existingUser = _context.Users.FirstOrDefault(s => s.Email == adminUserDTO.Email);

            if(existingUser != null)
            {
                // check for soft delete 
                if (existingUser.IsDeleted == true)
                {
                    existingUser.IsDeleted = false;
                    _context.Users.Update(existingUser);
                    await _context.SaveChangesAsync();
                    await _emailService.SendUserRegistrationEmailAsync(adminUserDTO.Email, adminUserDTO.Password);
                    return;
                }
                else
                {
                    throw new AlreadyExistsException<string>($"User is already exists with {adminUserDTO.Email}");
                }
            }

            // Hash the password
            var passwordHasher = new PasswordHasher<AdminUserDTO>();
            var hashedPassword = passwordHasher.HashPassword(adminUserDTO, adminUserDTO.Password);

            var newAdmin = new User
            {
                FirstName = adminUserDTO.FirstName,
                LastName = adminUserDTO.LastName,
                Email = adminUserDTO.Email,
                PhoneNo = adminUserDTO.PhoneNo,
                Password = hashedPassword,
                RoleId = 1,
                Active = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newAdmin);
            await _context.SaveChangesAsync();

            // Send Login credential into their registered mail
            await _emailService.SendUserRegistrationEmailAsync(adminUserDTO.Email, adminUserDTO.Password);
        }

        public async Task AddEmployeeQuery(EmplyeeUserDTO emplyeeUserDTO)
        {
            // check inserted role is exist or not
            var roleExists = await _context.Departments.AnyAsync(r => r.DepartmentId == emplyeeUserDTO.DepartmentId);

            if (!roleExists)
            {
                throw new Exception("Invalid RoleId. Role does not exist.");
            }

            // Check employee exist or not by their Email
            var existingEmployee = await _context.Users.FirstOrDefaultAsync(c => c.Email == emplyeeUserDTO.Email);

            if (existingEmployee != null)
            {
                // check employee is deleted or not
                if (existingEmployee.IsDeleted == true)
                {
                    existingEmployee.IsDeleted = false;
                    _context.Users.Update(existingEmployee);
                    await _context.SaveChangesAsync();
                    await _emailService.SendUserRegistrationEmailAsync(emplyeeUserDTO.Email, emplyeeUserDTO.Password);
                    return;
                }
                else
                {
                    throw new AlreadyExistsException<string>(emplyeeUserDTO.Email);
                }
            }

            // Hash the password
            var passwordHasher = new PasswordHasher<EmplyeeUserDTO>();
            var hashedPassword = passwordHasher.HashPassword(emplyeeUserDTO, emplyeeUserDTO.Password);

            // Map employeeDTO to User entity
            var user = _mapper.Map<User>(emplyeeUserDTO);
            user.Password = hashedPassword;  
            user.RoleId = 2;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            // Map employeeDTO to Employee entity
            var employee = _mapper.Map<Employee>(emplyeeUserDTO);
            employee.User = user;  // Establish relationship with User
            employee.DepartmentId = emplyeeUserDTO.DepartmentId;

            // Save to database
            await _context.Users.AddAsync(user);
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            await _emailService.SendUserRegistrationEmailAsync(emplyeeUserDTO.Email, emplyeeUserDTO.Password);
        }

        public async Task UpdateAdminByIdQuery(int id, AdminUserDTO adminUserDTO)
        {
            // check email already exist or not with same email but different id 
            var checkEmail = await _context.Users.FirstOrDefaultAsync(s => s.Email == adminUserDTO.Email && s.UserId != id);

            if (checkEmail != null)
                throw new AlreadyExistsException<string>($"User is Already exist with {adminUserDTO.Email}");

            // check admin exist or not
            var existingAdmin = await _context.Users.FirstOrDefaultAsync(s => s.UserId == id && s.RoleId == 1);

            if (existingAdmin == null)
                throw new DataNotFoundException<int>(id);

            existingAdmin.FirstName = adminUserDTO.FirstName ?? existingAdmin.FirstName;
            existingAdmin.LastName = adminUserDTO.LastName ?? existingAdmin.LastName;
            existingAdmin.Email = adminUserDTO.Email ?? existingAdmin.Email;
            existingAdmin.PhoneNo = adminUserDTO.PhoneNo ?? existingAdmin.PhoneNo;
            existingAdmin.Active = adminUserDTO.Active;
            existingAdmin.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(existingAdmin);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeByIdQuery(int id, EmplyeeUserDTO emplyeeUserDTO)
        {
            // Check if email is already in use by another user
            var checkEmail = await _context.Users
                .FirstOrDefaultAsync(s => s.Email == emplyeeUserDTO.Email && s.UserId != id);

            if (checkEmail != null)
                throw new AlreadyExistsException<string>(checkEmail.Email);

            // Fetch existing User with Employee details
            var existingUser = await _context.Users
                .Include(u => u.Employee)  // Ensure Employee entity is loaded
                .FirstOrDefaultAsync(u => u.UserId == id && u.RoleId == 2);

            if (existingUser == null)
                throw new DataNotFoundException<int>(id);

            // Use AutoMapper to update User entity
            _mapper.Map(emplyeeUserDTO, existingUser);
            existingUser.UpdatedAt = DateTime.UtcNow; // Ensure timestamp updates

            // Update Password only if a new one is provided
            if (!string.IsNullOrWhiteSpace(emplyeeUserDTO.Password))
            {
                var passwordHasher = new PasswordHasher<User>();
                existingUser.Password = passwordHasher.HashPassword(existingUser, emplyeeUserDTO.Password);
            }

            // Ensure Employee entity exists before modifying it
            if (existingUser.Employee != null)
            {
                _mapper.Map(emplyeeUserDTO, existingUser.Employee);

                // Update nullable fields separately to avoid overwriting with default values
                if (emplyeeUserDTO.DateOfBirth != default)
                    existingUser.Employee.DateOfBirth = emplyeeUserDTO.DateOfBirth;

                if (emplyeeUserDTO.JoinDate != default)
                    existingUser.Employee.JoinDate = emplyeeUserDTO.JoinDate;

                if (emplyeeUserDTO.RelievingDate != default)
                    existingUser.Employee.RelievingDate = emplyeeUserDTO.RelievingDate;
            }

            // Save changes (EF Core tracks updates automatically)
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserByIdQuery(int id)
        {
            // find aleady exist user
            var existingUser = await _context.Users.FirstOrDefaultAsync(c => c.UserId == id && c.IsDeleted == false);

            if (existingUser == null)
                throw new DataNotFoundException<int>(id);

            // Soft delete
            existingUser.IsDeleted = true;
            
            // change status as deactivate with deletion
            existingUser.Active = false;

            await _context.SaveChangesAsync();
        }
    }
}