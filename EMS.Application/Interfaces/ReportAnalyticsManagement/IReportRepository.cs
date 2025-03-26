using EMS_Backend_Project.EMS.Application.DTOs.ReportAnalyticsDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.ReportAnalyticsManagement
{
    public interface IReportRepository
    {
        Task<WeeklyWorkHoursReportDTO> GetWeeklyWorkHoursReportQuery(int employeeId, DateOnly date);
        Task<MonthlyWorkHoursReportDTO> GetMonthlyWorkHoursReportQuery(int employeeId, int month, int year);
        Task<ICollection<WeeklyWorkHoursReportDTO>> GetWeeklyReportOfAllEmployeeQuery(DateOnly date);
        Task<ICollection<MonthlyWorkHoursReportDTO>> GetMonthlyReportOfAllEmployeeQuery(int month, int year);
        Task<ICollection<MonthlyWorkHoursReportDTO>> GetCustomReportQuery(DateOnly startDate, DateOnly endDate);
    }
}