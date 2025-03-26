namespace EMS_Backend_Project.EMS.Application.DTOs.EmployeeDTOs
{
    public class GetEmployeeDataDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public string DepartmentName { get; set; }
        public string TeckStack { get; set; }
        public DateOnly JoinDate { get; set; }
    }
}
