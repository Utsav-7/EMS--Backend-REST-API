using EMS_Backend_Project.EMS.Application.DTOs.TimeSheetDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.TimeSheetManagement;
using EMS_Backend_Project.EMS.Common.CustomExceptions;
using EMS_Backend_Project.EMS.Domain.Entities;
using EMS_Backend_Project.EMS.Infrastructure.Database;
using EMS_Backend_Project.EMS.Infrastructure.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS_Backend_Project.EMS.Infrastructure.Repositories
{
    public class TimeSheetRepository : Repository<TimeSheet>, ITimeSheetRepository
    {
        public TimeSheetRepository(ApplicationDBContext context) : base(context) { }

        public async Task<ICollection<GetTimeSheetDTO>> GetAllSheetsQuery()
        {
            // Get all the Sheet that store in DB
            var sheetList = await _context.TimeSheets.Include(s => s.Employee)
                                                     .ThenInclude(u => u.User)
                                                     .ThenInclude(d => d.Employee.Department)
                                                     .Select(s => new GetTimeSheetDTO
                                                     {
                                                         TimeSheetId = s.TimeSheetId,
                                                         EmployeeName = s.Employee.User.FirstName + " " + s.Employee.User.LastName,
                                                         DepartmentName = s.Employee.Department.DepartmentName,
                                                         WorkDate = s.WorkDate,
                                                         StartTime = s.StartTime,
                                                         EndTime = s.EndTime,
                                                         BreakTime = s.BreakTime,
                                                         WorkHours = s.TotalHours,
                                                         Description = s.Description
                                                     })
                                                     .OrderByDescending(e => e.WorkDate)
                                                     .ToListAsync();

            if (sheetList == null)
                throw new DataNotFoundException<string>("No Time sheet found.");

            return sheetList;
        }

        public async Task<ICollection<GetEmployeeSheetDTO>> GetSheetByIdQuery(int userId)
        {
            // get timesheet by userId for employee 
            var getSheetList = await _context.TimeSheets.Include(e => e.Employee)
                                                        .Where(s => s.Employee.UserId == userId)
                                                        .Select(s => new GetEmployeeSheetDTO
                                                        {
                                                            TimeSheetId = s.TimeSheetId,
                                                            EmployeeId = s.EmployeeId,
                                                            WorkDate = s.WorkDate,
                                                            StartTime = s.StartTime,
                                                            EndTime = s.EndTime,
                                                            BreakTime = s.BreakTime,
                                                            TotalWorkHours = s.TotalHours,
                                                            Description = s.Description
                                                        })
                                                        .OrderBy(d => d.WorkDate)
                                                        .ToListAsync();

            if (getSheetList == null)
                throw new DataNotFoundException<int>(userId);

            return getSheetList;
        }

        public async Task<GetTimeSheetDTO> GetSheetByIdAndDateQuery(int employeeId, DateOnly workDate)

        {
            // Get sheet by EmployeeID and Date
            var sheet = await _context.TimeSheets.Include(s => s.Employee)
                                                                 .ThenInclude(u => u.User)
                                                                 .ThenInclude(d => d.Employee.Department)
                                                                 .Where(c => c.EmployeeId == employeeId && c.WorkDate == workDate)
                                                                 .Select(s => new GetTimeSheetDTO
                                                                 {
                                                                     TimeSheetId = s.TimeSheetId,
                                                                     EmployeeName = s.Employee.User.FirstName + " " + s.Employee.User.LastName,
                                                                     DepartmentName = s.Employee.Department.DepartmentName,
                                                                     WorkDate = s.WorkDate,
                                                                     StartTime = s.StartTime,
                                                                     EndTime = s.EndTime,
                                                                     BreakTime = s.BreakTime,
                                                                     WorkHours = s.TotalHours,
                                                                     Description = s.Description
                                                                 }).FirstOrDefaultAsync();

            if (sheet == null)
                throw new DataNotFoundException<int>(employeeId);

            return sheet;
        }

        public async Task AddSheetQuery(int userId, TimeSheetDTO timeSheet)
        {
            // Check Timesheet already exist in db 
            var existingSheet = _context.TimeSheets.FirstOrDefault(s => (s.Employee.UserId == userId && s.WorkDate == timeSheet.WorkDate) ||  (s.Employee.UserId == timeSheet.EmployeeId && s.WorkDate == timeSheet.WorkDate));

            var employee = await _context.Employees.FirstOrDefaultAsync(s => s.UserId == userId || s.UserId == timeSheet.EmployeeId);
            var newId = existingSheet?.EmployeeId ?? timeSheet.EmployeeId;

            if (existingSheet != null)
                throw new AlreadyExistsException<string>("Time Sheet already Exists.");

            if (employee == null)
            {
                throw new DataNotFoundException<int>(userId);
            }

            var newSheet = new TimeSheet
            {
                EmployeeId = newId,
                WorkDate = timeSheet.WorkDate,
                StartTime = timeSheet.StartTime,
                EndTime = timeSheet.EndTime,
                BreakTime = timeSheet.BreakTime,
                Description = timeSheet.Description,
                CreatedAt = DateTime.UtcNow,
                TotalHours = (timeSheet.EndTime - timeSheet.StartTime - timeSheet.BreakTime)
            };

            _context.TimeSheets.Add(newSheet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSheetQuery(int id, TimeSheetDTO timeSheet)
        {
            // chech sheet is exist or not
            var existingSheet = await _context.TimeSheets.Include(e => e.Employee)
                                                         .Where(s => (s.EmployeeId == id && s.WorkDate == timeSheet.WorkDate) ||  (s.Employee.UserId == id && s.WorkDate == timeSheet.WorkDate)).FirstOrDefaultAsync();

            if (existingSheet == null)
                throw new DataNotFoundException<int>(id);

            existingSheet.EmployeeId = existingSheet.EmployeeId;
            existingSheet.WorkDate = timeSheet.WorkDate;
            existingSheet.StartTime = timeSheet.StartTime;
            existingSheet.EndTime = timeSheet.EndTime;
            existingSheet.BreakTime = timeSheet.BreakTime;
            existingSheet.TotalHours = (timeSheet.EndTime - timeSheet.StartTime - timeSheet.BreakTime);
            existingSheet.Description = timeSheet.Description;

            _context.TimeSheets.Update(existingSheet);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteSheetQuery(int id, DateOnly date)
        {
            // find existing sheet
            var existingSheet = await _context.TimeSheets.FirstOrDefaultAsync(s => s.EmployeeId == id && s.WorkDate == date);

            if (existingSheet == null)
                throw new DataNotFoundException<int>(id);

            _context.TimeSheets.Remove(existingSheet);
            await _context.SaveChangesAsync();
        }

        public async Task<FileContentResult> ExportAllRecordsQuery()
        {
            // Get sheet list of all sheets
            var sheetList = await _context.TimeSheets.Include(s => s.Employee)
                                         .ThenInclude(u => u.User)
                                         .ThenInclude(d => d.Employee.Department)
                                         .Select(s => new
                                         {
                                             TimeSheetId = s.TimeSheetId,
                                             EmployeeName = s.Employee.User.FirstName + " " + s.Employee.User.LastName,
                                             DepartmentName = s.Employee.Department.DepartmentName,
                                             WorkDate = s.WorkDate.ToString("yyyy-MM-dd"),  // Format WorkDate
                                             StartTime = s.StartTime.ToString(@"hh\:mm"),    // Format StartTime
                                             EndTime = s.EndTime.ToString(@"hh\:mm"),        // Format EndTime
                                             BreakTime = s.BreakTime.ToString(@"hh\:mm"),    // Format BreakTime
                                             WorkHours = s.TotalHours.ToString(@"hh\:mm"),   // Format WorkHours
                                             Description = s.Description
                                         }).ToListAsync();


            byte[] fileBytes = ExcelExportHelper.ExportToExcel(sheetList);

            return new FileContentResult(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "TimeSheet.xlsx"
            };
        }

        public async Task<FileContentResult> ExportAllRecordsByIdQuery(int empId)
        {
            // get sheet list of specific user by id
            var sheetList = await _context.TimeSheets.Include(s => s.Employee)
                                         .ThenInclude(u => u.User)
                                         .ThenInclude(d => d.Employee.Department)
                                         .Where(c => c.EmployeeId == empId)
                                         .Select(s => new
                                         {
                                             TimeSheetId = s.TimeSheetId,
                                             EmployeeName = s.Employee.User.FirstName + " " + s.Employee.User.LastName,
                                             DepartmentName = s.Employee.Department.DepartmentName,
                                             WorkDate = s.WorkDate.ToString("yyyy-MM-dd"),  // Format WorkDate
                                             StartTime = s.StartTime.ToString(@"hh\:mm"),    // Format StartTime
                                             EndTime = s.EndTime.ToString(@"hh\:mm"),        // Format EndTime
                                             BreakTime = s.BreakTime.ToString(@"hh\:mm"),    // Format BreakTime
                                             WorkHours = s.TotalHours.ToString(@"hh\:mm"),   // Format WorkHours
                                             Description = s.Description
                                         }).ToListAsync();


            byte[] fileBytes = ExcelExportHelper.ExportToExcel(sheetList);

            return new FileContentResult(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "TimeSheet.xlsx"
            };
        }
    }
}