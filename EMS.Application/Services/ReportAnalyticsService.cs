using EMS_Backend_Project.EMS.Application.DTOs.ReportAnalyticsDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.ReportAnalyticsManagement;

namespace EMS_Backend_Project.EMS.Application.Services
{
    public class ReportAnalyticsService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportAnalyticsService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public Task<ICollection<MonthlyWorkHoursReportDTO>> GetCustomReportAsync(DateOnly startDate, DateOnly endDate)
        {
            return _reportRepository.GetCustomReportQuery(startDate, endDate);
        }

        public Task<ICollection<MonthlyWorkHoursReportDTO>> GetMonthlyReportOfAllEmployeeAsync(int month, int year)
        {
            return _reportRepository.GetMonthlyReportOfAllEmployeeQuery(month, year);
        }

        public Task<MonthlyWorkHoursReportDTO> GetMonthlyWorkHoursReportAsync(int employeeId, int month, int year)
        {
            return _reportRepository.GetMonthlyWorkHoursReportQuery(employeeId, month, year);
        }

        public Task<ICollection<WeeklyWorkHoursReportDTO>> GetWeeklyReportOfAllEmployeeAsync(DateOnly date)
        {
            return _reportRepository.GetWeeklyReportOfAllEmployeeQuery(date);
        }

        public Task<WeeklyWorkHoursReportDTO> GetWeeklyWorkHoursReportAsync(int employeeId, DateOnly date)
        {
            return _reportRepository.GetWeeklyWorkHoursReportQuery(employeeId, date);
        }
    }
}