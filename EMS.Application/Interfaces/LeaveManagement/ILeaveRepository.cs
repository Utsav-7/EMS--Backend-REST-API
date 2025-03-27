using EMS_Backend_Project.EMS.Application.DTOs.LeavesDTOs;

namespace EMS_Backend_Project.EMS.Application.Interfaces.LeaveManagement
{
    public interface ILeaveRepository
    {
        Task<ICollection<GetLeaveDTO>> GetAllLeavesQuery();
        Task<ICollection<GetLeaveDTO>> GetLeaveByIDQuery(int id);
        Task AddLeaveQuery(int employeeId, LeaveDTO leave);
        Task UpdateLeaveQuery(int leaveId, LeaveDTO leave);
        Task DeleteLeaveQuery(int leaveId);
    }
}