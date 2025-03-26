namespace EMS_Backend_Project.EMS.Application.DTOs.ReportAnalyticsDTOs
{
    public class MonthlyWorkHoursReportDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public double TotalHours { get; set; }
        public int TotalLeaveDays { get; set; }
        public DateOnly MonthStartDate { get; set; }
        public DateOnly MonthEndDate { get; set; }
    }
}
