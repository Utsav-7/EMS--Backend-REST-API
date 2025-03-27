using EMS_Backend_Project.EMS.Application.DTOs.ReportAnalyticsDTOs;
using EMS_Backend_Project.EMS.Application.DTOs.TimeSheetDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.ReportAnalyticsManagement;
using EMS_Backend_Project.EMS.Common.CustomExceptions;
using EMS_Backend_Project.EMS.Domain.Entities;
using EMS_Backend_Project.EMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EMS_Backend_Project.EMS.Infrastructure.Repositories
{
    public class ReportRepository : Repository<TimeSheet>, IReportRepository
    {
        public ReportRepository(ApplicationDBContext context) : base(context){ }

        public async Task<MonthlyWorkHoursReportDTO> GetMonthlyWorkHoursReportQuery(int employeeId, int month, int year)
        {
            // Calculate date range for the month
            DateOnly firstDayOfMonth = DateOnly.FromDateTime(new DateTime(year, month, 1));
            DateOnly lastDayOfMonth = DateOnly.FromDateTime(new DateTime(year, month, DateTime.DaysInMonth(year, month)));

            // Get employee details with related data
            var employee = await _context.Employees
                                         .Include(e => e.User)
                                         .Include(e => e.Department)
                                         .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
                throw new DataNotFoundException<string>($"No employee found with id {employeeId}");

            // Get timesheets for the specified month and year
            var monthlyTimeSheets = await _context.TimeSheets
                                                  .Where(ts => ts.EmployeeId == employeeId &&
                                                              ts.WorkDate.Month == month &&
                                                              ts.WorkDate.Year == year)
                                                  .ToListAsync();

            // Get approved leaves that overlap with the month
            var monthlyLeaves = await _context.Leaves
                                              .Where(l => l.EmployeeId == employeeId &&
                                                          l.Status == "Approved" &&
                                                          l.StartDate <= lastDayOfMonth &&
                                                          l.EndDate >= firstDayOfMonth)
                                              .ToListAsync();

            // Calculate actual leave days within the month
            var totalLeaveDays = monthlyLeaves.Sum(l =>
                                {
                                    var leaveStart = l.StartDate > firstDayOfMonth ? l.StartDate : firstDayOfMonth;
                                    var leaveEnd = l.EndDate < lastDayOfMonth ? l.EndDate : lastDayOfMonth;
                                    return (leaveEnd.DayNumber - leaveStart.DayNumber) + 1; // Inclusive count
                                });

            // Create the report
            var report = new MonthlyWorkHoursReportDTO
            {
                EmployeeId = employeeId,
                EmployeeName = $"{employee.User.FirstName} {employee.User.LastName}",
                Department = employee.Department?.DepartmentName,
                TotalHours = monthlyTimeSheets.Sum(ts => ts.TotalHours.TotalHours),
                TotalLeaveDays = totalLeaveDays,
                MonthStartDate = firstDayOfMonth,
                MonthEndDate = lastDayOfMonth
            };

            return report;
        }

        public async Task<ICollection<MonthlyWorkHoursReportDTO>> GetMonthlyReportOfAllEmployeeQuery(int month, int year)
        {
            // Calculate date range for the month
            DateOnly firstDayOfMonth = DateOnly.FromDateTime(new DateTime(year, month, 1));
            DateOnly lastDayOfMonth = DateOnly.FromDateTime(new DateTime(year, month, DateTime.DaysInMonth(year, month)));

            // Get all employees with related data
            var employees = await _context.Employees
                                          .Include(e => e.User)
                                          .Include(e => e.Department)
                                          .ToListAsync();

            if (employees == null || !employees.Any())
                throw new DataNotFoundException<string>("No Employees found.");

            List<MonthlyWorkHoursReportDTO> reportList = new();

            foreach (var employee in employees)
            {
                // Get time sheets for the employee within the specified month
                var monthlyTimeSheets = await _context.TimeSheets
                                                      .Where(ts => ts.EmployeeId == employee.EmployeeId &&
                                                                   ts.WorkDate.Month == month &&
                                                                   ts.WorkDate.Year == year)
                                                      .ToListAsync();

                // Get approved leaves for the employee within the month
                var monthlyLeaves = await _context.Leaves
                                                  .Where(l => l.EmployeeId == employee.EmployeeId &&
                                                              l.Status == "Approved" &&
                                                              l.StartDate <= lastDayOfMonth &&
                                                              l.EndDate >= firstDayOfMonth)
                                                  .ToListAsync();

                // Calculate actual leave days within the month
                var totalLeaveDays = monthlyLeaves.Sum(l =>
                {
                    var leaveStart = l.StartDate > firstDayOfMonth ? l.StartDate : firstDayOfMonth;
                    var leaveEnd = l.EndDate < lastDayOfMonth ? l.EndDate : lastDayOfMonth;
                    return (leaveEnd.DayNumber - leaveStart.DayNumber) + 1; // Inclusive count
                });

                // Add to report list
                reportList.Add(new MonthlyWorkHoursReportDTO
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = $"{employee.User.FirstName} {employee.User.LastName}",
                    Department = employee.Department?.DepartmentName,
                    TotalHours = monthlyTimeSheets.Sum(ts => ts.TotalHours.TotalHours),
                    TotalLeaveDays = totalLeaveDays,
                    MonthStartDate = firstDayOfMonth,
                    MonthEndDate = lastDayOfMonth
                });
            }

            return reportList;
        }

        public async Task<WeeklyWorkHoursReportDTO> GetWeeklyWorkHoursReportQuery(int employeeId, DateOnly chooseDate)
        {
            // set Monday-Sunday Week Date based on given date
            DateOnly startOfWeek = chooseDate.AddDays(-(int)chooseDate.DayOfWeek + (int)DayOfWeek.Monday);
            DateOnly endOfWeek = startOfWeek.AddDays(6);

            // Get employee details with related data
            var employee = await _context.Employees
                                         .Include(e => e.User)
                                         .Include(e => e.Department)
                                         .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
                throw new DataNotFoundException<string>($"No employee found with id {employeeId}");

            // Get time sheet for the specific employee within the week
            var weeklySheet = await _context.TimeSheets
                                            .Where(ts => ts.EmployeeId == employeeId &&
                                                         ts.WorkDate >= startOfWeek &&
                                                         ts.WorkDate <= endOfWeek)
                                            .Select(s => new GetTimeSheetDTO
                                            {
                                                TimeSheetId = s.TimeSheetId,
                                                EmployeeName = $"{s.Employee.User.FirstName} {s.Employee.User.LastName}" ,
                                                DepartmentName = s.Employee.Department.DepartmentName,
                                                WorkDate = s.WorkDate,
                                                StartTime = s.StartTime,
                                                BreakTime = s.BreakTime,
                                                EndTime = s.EndTime,
                                                WorkHours = s.TotalHours,
                                                Description = s.Description
                                            })
                                            .ToListAsync();

            // Get approved leaves for the employee within the same week
            var weeklyLeaves = await _context.Leaves
                                              .Where(l => l.EmployeeId == employeeId &&
                                                          l.Status == "Approved" &&
                                                          l.StartDate <= endOfWeek &&
                                                          l.EndDate >= startOfWeek)
                                              .ToListAsync();

            // Calculate actual leave days within the week
            int totalLeaveDays = weeklyLeaves.Sum(l => (Math.Min(l.EndDate.DayNumber, endOfWeek.DayNumber) -
                                                         Math.Max(l.StartDate.DayNumber, startOfWeek.DayNumber)) + 1);

            // Create report DTO
            return new WeeklyWorkHoursReportDTO
            {
                EmployeeId = employeeId,
                EmployeeName = $"{employee.User.FirstName} {employee.User.LastName}",
                Department = employee.Department?.DepartmentName,
                TotalHours = weeklySheet.Sum(ts => ts.WorkHours.TotalHours),
                TotalLeaveDays = totalLeaveDays,
                WeekStartDate = startOfWeek,
                WeekEndDate = endOfWeek,
                SheetList = weeklySheet
            };
        }

        public async Task<ICollection<WeeklyWorkHoursReportDTO>> GetWeeklyReportOfAllEmployeeQuery(DateOnly chooseDate)
        {
            // Set Monday-Sunday Week date based on given date
            DateOnly startOfWeek = chooseDate.AddDays(-(int)chooseDate.DayOfWeek + (int)DayOfWeek.Monday);
            DateOnly endOfWeek = startOfWeek.AddDays(6);

            // Get all employees with related data
            var employees = await _context.Employees
                                          .Include(e => e.User)
                                          .Include(e => e.Department)
                                          .ToListAsync();

            List<WeeklyWorkHoursReportDTO> reportCollection = new();

            foreach (var employee in employees)
            {
                // Get time sheets for the specific employee within the week
                var weeklySheet = await _context.TimeSheets
                                                .Where(ts => ts.EmployeeId == employee.EmployeeId &&
                                                             ts.WorkDate >= startOfWeek &&
                                                             ts.WorkDate <= endOfWeek)
                                                .Select(s => new GetTimeSheetDTO
                                                {
                                                    TimeSheetId = s.TimeSheetId,
                                                    EmployeeName = $"{s.Employee.User.FirstName} {s.Employee.User.LastName}",
                                                    DepartmentName = s.Employee.Department.DepartmentName,
                                                    WorkDate = s.WorkDate,
                                                    StartTime = s.StartTime,
                                                    BreakTime = s.BreakTime,
                                                    EndTime = s.EndTime,
                                                    WorkHours = s.TotalHours,
                                                    Description = s.Description
                                                })
                                                .ToListAsync();

                // Get approved leaves for the employee within the same week
                var weeklyLeaves = await _context.Leaves
                                                 .Where(l => l.EmployeeId == employee.EmployeeId &&
                                                             l.Status == "Approved" &&
                                                             l.StartDate <= endOfWeek &&
                                                             l.EndDate >= startOfWeek)
                                                 .ToListAsync();

                // Calculate actual leave days within the week
                int totalLeaveDays = weeklyLeaves.Sum(l => (Math.Min(l.EndDate.DayNumber, endOfWeek.DayNumber) -
                                                             Math.Max(l.StartDate.DayNumber, startOfWeek.DayNumber)) + 1);

                // Create report DTO for the employee
                var report = new WeeklyWorkHoursReportDTO
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = $"{employee.User.FirstName} {employee.User.LastName}",
                    Department = employee.Department.DepartmentName,
                    TotalHours = weeklySheet.Sum(ts => ts.WorkHours.TotalHours),
                    TotalLeaveDays = totalLeaveDays,
                    WeekStartDate = startOfWeek,
                    WeekEndDate = endOfWeek,
                    SheetList = weeklySheet
                };

                reportCollection.Add(report);
            }

            return reportCollection;
        }

        public async Task<ICollection<MonthlyWorkHoursReportDTO>> GetCustomReportQuery(DateOnly startDate, DateOnly endDate)
        {
            // Calculate date range for the month
            DateOnly firstDate = startDate;
            DateOnly lastDate = endDate;

            if (startDate > endDate)
                throw new Exception("End Date must be the same as Start Date or greater than.");

            // Get all employees with related data
            var employees = await _context.Employees
                                          .Include(e => e.User)
                                          .Include(e => e.Department)
                                          .ToListAsync();

            List<MonthlyWorkHoursReportDTO> reportList = new();

            foreach (var employee in employees)
            {
                // Get time sheets for the employee within the specified month
                var TimeSheetList = await _context.TimeSheets
                                                  .Where(ts => ts.EmployeeId == employee.EmployeeId &&
                                                               ts.WorkDate >= startDate &&
                                                               ts.WorkDate <= endDate)
                                                  .ToListAsync();

                // Get approved leaves for the employee within the month
                var LeavesList = await _context.Leaves
                                                  .Where(l => l.EmployeeId == employee.EmployeeId &&
                                                              l.Status == "Approved" &&
                                                              l.StartDate <= lastDate &&
                                                              l.EndDate >= firstDate)
                                                  .ToListAsync();

                // Calculate actual leave days within the month
                var totalLeaveDays = LeavesList.Sum(l =>
                {
                    var leaveStart = l.StartDate > firstDate ? l.StartDate : firstDate;
                    var leaveEnd = l.EndDate < lastDate ? l.EndDate : lastDate ;
                    return (leaveEnd.DayNumber - leaveStart.DayNumber) + 1; // Inclusive count
                });

                // Add to report list
                reportList.Add(new MonthlyWorkHoursReportDTO
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = $"{employee.User.FirstName} {employee.User.LastName}",
                    Department = employee.Department?.DepartmentName,
                    TotalHours = TimeSheetList.Sum(ts => ts.TotalHours.TotalHours),
                    TotalLeaveDays = totalLeaveDays,
                    MonthStartDate = firstDate,
                    MonthEndDate = lastDate
                });
            }

            return reportList;
        }
    }
}