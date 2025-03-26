using System.ComponentModel.DataAnnotations;
using EMS_Backend_Project.EMS.Domain.Common.Validation;

namespace EMS_Backend_Project.EMS.Domain.Entities
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [CustomStringLength(20)]
        public required string RoleName { get; set; }

        // Navigation Property
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}