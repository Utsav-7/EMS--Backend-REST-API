using System;
using System.ComponentModel.DataAnnotations;
using EMS_Backend_Project.EMS.Application.DTOs.UserDTOs;
using EMS_Backend_Project.EMS.Domain.Common.Validation;
using EMS_Backend_Project.EMS.Domain.Common.Validators;

namespace EMS_Backend_Project.EMS.Application.DTOs.UserDTOs
{
    public class EmplyeeUserDTO : AdminUserDTO
    {
        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(250, ErrorMessage = "Address cannot be more than 250 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Department ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Department ID must be a positive number.")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Tech Stack is required.")]
        [StringLength(100, ErrorMessage = "Tech Stack cannot exceed 100 characters.")]
        public string TechStack { get; set; }

        [Required(ErrorMessage = "Join Date is required.")]
        public DateOnly JoinDate { get; set; }

        [Required(ErrorMessage = "Relieving Date is required.")]
        [DateGreaterThan(nameof(JoinDate), ErrorMessage = "Relieving Date must be greater than Join Date.")]
        public DateOnly RelievingDate { get; set; }
    }
}
