using EMS_Backend_Project.EMS.Application.DTOs.ReportAnalyticsDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.ReportAnalyticsManagement
{
    public interface IReportService
    {
        Task<WeeklyWorkHoursReportDTO> GetWeeklyWorkHoursReportAsync(int employeeId, DateOnly date);
        Task<MonthlyWorkHoursReportDTO> GetMonthlyWorkHoursReportAsync(int employeeId, int month, int year);
        Task<ICollection<WeeklyWorkHoursReportDTO>> GetWeeklyReportOfAllEmployeeAsync(DateOnly date);
        Task<ICollection<MonthlyWorkHoursReportDTO>> GetMonthlyReportOfAllEmployeeAsync(int month, int year);
        Task<ICollection<MonthlyWorkHoursReportDTO>> GetCustomReportAsync(DateOnly startDate, DateOnly endDate);
    }
}