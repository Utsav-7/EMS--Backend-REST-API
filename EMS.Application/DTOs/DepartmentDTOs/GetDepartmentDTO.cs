namespace EMS_Backend_Project.EMS.Application.DTOs.DepartmentDTOs
{
    public class GetDepartmentDTO
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TotalEmployee { get; set; }
    }
}