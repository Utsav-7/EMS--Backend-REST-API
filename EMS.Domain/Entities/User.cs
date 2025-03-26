using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EMS_Backend_Project.EMS.Domain.Common.Validation;
using Microsoft.EntityFrameworkCore;

namespace EMS_Backend_Project.EMS.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [CustomStringLength(30)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [CustomStringLength(30)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone No. is required.")]
        [Phone]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [ForeignKey("Role")] 
        public int RoleId { get; set; }

        public bool Active { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Role Role { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}