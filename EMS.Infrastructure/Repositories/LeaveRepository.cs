using EMS_Backend_Project.EMS.Application.DTOs.LeavesDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.LeaveManagement;
using EMS_Backend_Project.EMS.Common.CustomExceptions;
using EMS_Backend_Project.EMS.Domain.Entities;
using EMS_Backend_Project.EMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EMS_Backend_Project.EMS.Infrastructure.Repositories
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        public LeaveRepository(ApplicationDBContext context) : base(context){}

        public async Task<ICollection<GetLeaveDTO>> GetAllLeavesQuery()
        {
            // get all employee leaves
            var leaveRecords = await _context.Leaves
                                            .Include(s => s.Employee)
                                                .ThenInclude(e => e.User)
                                            .Include(s => s.Employee.Department)
                                            .Select(s => new GetLeaveDTO
                                            {
                                                LeaveId = s.LeaveId,
                                                EmployeeName = s.Employee.User.FirstName + " " + s.Employee.User.LastName,
                                                DepartmentName = s.Employee.Department.DepartmentName,
                                                StartDate = s.StartDate,
                                                EndDate = s.EndDate,
                                                TotalDays = s.TotalDays,
                                                LeaveType = s.LeaveType,
                                                Reason = s.Reason,
                                                Status = s.Status,
                                                AppliedAt = s.AppliedAt
                                            }).ToListAsync();

            if (leaveRecords == null)
                throw new DataNotFoundException<string>("No any Leave record found.");

            return leaveRecords;
        }
        public async Task<ICollection<GetLeaveDTO>> GetLeaveByIDQuery(int id)
        {
            // get specific employees' leave
            var leaveRecord = await _context.Leaves.Include(s => s.Employee)
                                                                .ThenInclude(u => u.User)
                                                                .ThenInclude(d => d.Employee.Department)
                                                                .Where(c => c.Employee.UserId == id)
                                                                .Select(s => new GetLeaveDTO
                                                                {
                                                                    LeaveId = s.LeaveId,
                                                                    EmployeeName = s.Employee.User.FirstName + " " + s.Employee.User.LastName,
                                                                    DepartmentName = s.Employee.Department.DepartmentName,
                                                                    StartDate = s.StartDate,
                                                                    EndDate = s.EndDate,
                                                                    TotalDays = s.TotalDays,
                                                                    LeaveType = s.LeaveType,
                                                                    Reason = s.Reason,
                                                                    Status = s.Status,
                                                                    AppliedAt = s.AppliedAt
                                                                }).ToListAsync();

            if (leaveRecord == null || leaveRecord.Count == 0)
                throw new DataNotFoundException<int>(id);

            return leaveRecord;
        }
        public async Task AddLeaveQuery(int loggedUserID, LeaveDTO leave)
        {
            // Get employee Id of particular logged employee or If Admin then take employeeID directly
            var employee = await _context.Employees
                                         .Include(e => e.Leaves)
                                         .FirstOrDefaultAsync(s => s.UserId == loggedUserID || s.EmployeeId == leave.EmployeeId);

            // update employee Id based in logged User (Admin / Employee)
            int newId = employee?.EmployeeId ?? leave.EmployeeId;

            if (employee == null)
            {
                throw new DataNotFoundException<int>(newId);
            }

            // Check if any existing leave matches the new leave request
            bool isLeaveAlreadyApplied = employee.Leaves
                .Any(l => l.StartDate == leave.StartDate && l.EndDate == leave.EndDate);

            if (isLeaveAlreadyApplied)
            {
                throw new AlreadyExistsException<string>("Leave already applied.");
            }

            var newLeave = new Leave
            {
                EmployeeId = newId,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                TotalDays = (leave.EndDate.ToDateTime(TimeOnly.MinValue) - leave.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1,
                LeaveType = leave.LeaveType,
                Reason = leave.Reason,
                Status = "Pending",
                AppliedAt = DateTime.UtcNow,
                UpdatedAt = null
            };
            _context.Leaves.Add(newLeave);

            await _context.SaveChangesAsync();
        }
        public async Task UpdateLeaveQuery(int id, LeaveDTO leave)
        {
            // Check leave is exist or not
            var existingRecord = await _context.Leaves.FindAsync(id);

            if (existingRecord == null)
                throw new DataNotFoundException<int>(id);

            existingRecord.EmployeeId = leave.EmployeeId;
            existingRecord.StartDate = leave.StartDate;
            existingRecord.EndDate = leave.EndDate;
            existingRecord.LeaveType = leave.LeaveType;
            existingRecord.Reason = leave.Reason;
            existingRecord.Status = leave.Status;
            existingRecord.TotalDays = (leave.EndDate.ToDateTime(TimeOnly.MinValue) - leave.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1;
            existingRecord.UpdatedAt = DateTime.UtcNow;

            _context.Leaves.Update(existingRecord);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteLeaveQuery(int leaveId)
        {
            var existingLeave = await _context.Leaves.FindAsync(leaveId);

            if (existingLeave == null)
                throw new DataNotFoundException<int>(leaveId);

            // delete leave (Hard Delete)
            _context.Leaves.Remove(existingLeave);
            await _context.SaveChangesAsync();
        }
    }
}