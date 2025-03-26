using EMS_Backend_Project.EMS.Application.DTOs.TimeSheetDTOs;
using EMS_Backend_Project.EMS.Domain.Entities;

namespace EMS_Backend_Project.EMS.Application.DTOs.ReportAnalyticsDTOs
{
    public class WeeklyWorkHoursReportDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public double TotalHours { get; set; }
        public int TotalLeaveDays { get; set; }
        public DateOnly WeekStartDate { get; set; }
        public DateOnly WeekEndDate { get; set; }
        public virtual List<GetTimeSheetDTO> SheetList { get; set; }
    }
}