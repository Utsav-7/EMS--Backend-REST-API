namespace EMS_Backend_Project.EMS.Application.DTOs.LeavesDTOs
{
    public class GetLeaveDTO
    {
        public int LeaveId { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int TotalDays { get; set; }
        public string LeaveType { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}