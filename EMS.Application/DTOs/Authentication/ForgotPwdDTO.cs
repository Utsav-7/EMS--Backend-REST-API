using System.ComponentModel.DataAnnotations;

namespace EMS_Backend_Project.EMS.Application.DTOs.Authentication
{
    public class ForgotPwdDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email length can't be more than 100 characters.")]
        public string Email { get; set; }
    }
}