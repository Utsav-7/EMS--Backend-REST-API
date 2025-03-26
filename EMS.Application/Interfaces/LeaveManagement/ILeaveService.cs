using EMS_Backend_Project.EMS.Application.DTOs.LeavesDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.LeaveManagement
{
    public interface ILeaveService
    {
        Task<ICollection<GetLeaveDTO>> GetAllLeavesAsync();
        Task<ICollection<GetLeaveDTO>> GetLeaveByIDAsync(int id);
        Task AddLeaveAsync(int id, LeaveDTO leave);
        Task UpdateLeaveAsync(int id, LeaveDTO leave);
        Task DeleteLeaveAsync(int id);
    }
}
