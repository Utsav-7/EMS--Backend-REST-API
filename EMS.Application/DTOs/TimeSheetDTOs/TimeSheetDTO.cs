using EMS_Backend_Project.EMS.Domain.Common.Validators;
using System.ComponentModel.DataAnnotations;

namespace EMS_Backend_Project.EMS.Application.DTOs.TimeSheetDTOs
{
    public class TimeSheetDTO
    {
        [Required(ErrorMessage = "Employee ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Employee ID must be a positive number.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Work date is required.")]
        public DateOnly WorkDate { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        [TimeAfter(nameof(StartTime), ErrorMessage = "End time must be after Start time.")]
        public TimeSpan EndTime { get; set; }

        [Range(typeof(TimeSpan), "00:00:00", "12:00:00", ErrorMessage = "Break time must be between 0 and 12 hours.")]
        public TimeSpan BreakTime { get; set; } = TimeSpan.Zero;

        [StringLength(500, ErrorMessage = "Description can't be more than 500 characters.")]
        public string? Description { get; set; }
    }
}