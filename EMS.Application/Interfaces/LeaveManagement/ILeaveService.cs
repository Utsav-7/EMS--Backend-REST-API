using EMS_Backend_Project.EMS.Application.DTOs.LeavesDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.LeaveManagement
{
    public interface ILeaveService
    {
        Task<ICollection<GetLeaveDTO>> GetAllLeavesAsync();
        Task<ICollection<GetLeaveDTO>> GetLeaveByIDAsync(int id);
        Task AddLeaveAsync(int employeeId, LeaveDTO leave);
        Task UpdateLeaveAsync(int leaveId, LeaveDTO leave);
        Task DeleteLeaveAsync(int leaveId);
    }
}