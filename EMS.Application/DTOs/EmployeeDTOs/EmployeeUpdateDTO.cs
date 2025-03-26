using System.ComponentModel.DataAnnotations;

namespace EMS_Backend_Project.EMS.Application.DTOs.EmployeeDTOs
{
    public class EmployeeUpdateDTO
    {
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits and should not include a country code.")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Tech Stack is required.")]
        [StringLength(100, ErrorMessage = "Tech Stack can't be more than 100 characters.")]
        public string TechStack { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255, ErrorMessage = "Address can't be more than 255 characters.")]
        public string Address { get; set; }
    }
}
