namespace EMS_Backend_Project.EMS.Application.DTOs.TimeSheetDTOs
{
    public class GetTimeSheetDTO
    {
        public int TimeSheetId { get; set; }    
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public DateOnly WorkDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan BreakTime { get; set; }
        public TimeSpan WorkHours { get; set; }
        public string? Description { get; set; }
    }
}