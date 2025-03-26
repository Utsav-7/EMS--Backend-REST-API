namespace EMS_Backend_Project.EMS.Application.DTOs.TimeSheetDTOs
{
    public class GetEmployeeSheetDTO
    {
        public int TimeSheetId { get; set; }
        public int EmployeeId { get; set; }
        public DateOnly WorkDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan BreakTime { get; set; }
        public TimeSpan TotalWorkHours { get; set; }
        public string? Description { get; set; }
    }
}