using EMS_Backend_Project.EMS.Application.DTOs.LeavesDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.LeaveManagement;

namespace EMS_Backend_Project.EMS.Application.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public Task AddLeaveAsync(int loggedUserId, LeaveDTO leave)
        {
            return _leaveRepository.AddLeaveQuery(loggedUserId, leave);
        }

        public Task DeleteLeaveAsync(int id)
        {
            return _leaveRepository.DeleteLeaveQuery(id);
        }

        public async Task<ICollection<GetLeaveDTO>> GetAllLeavesAsync()
        {
            return await _leaveRepository.GetAllLeavesQuery();
        }

        public async Task<ICollection<GetLeaveDTO>> GetLeaveByIDAsync(int id)
        {
            return await _leaveRepository.GetLeaveByIDQuery(id);
        }

        public Task UpdateLeaveAsync(int id, LeaveDTO leave)
        {
            return _leaveRepository.UpdateLeaveQuery(id, leave);
        }
    }
}
