using EMS_Backend_Project.EMS.Application.DTOs.TimeSheetDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.TimeSheetManagement;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend_Project.EMS.Application.Services
{
    public class TimeSheetService : ITimeSheetService
    {
        private readonly ITimeSheetRepository _timeSheetRepository;

        public TimeSheetService(ITimeSheetRepository timeSheetRepository)
        {
            _timeSheetRepository = timeSheetRepository;
        }

        public Task AddSheetAsync(int userId, TimeSheetDTO timeSheet)
        {
            return _timeSheetRepository.AddSheetQuery(userId, timeSheet);
        }

        public Task DeleteSheetAsync(int id, DateOnly workDate)
        {
            return _timeSheetRepository.DeleteSheetQuery(id, workDate);
        }

        public Task<FileContentResult> ExportAllRecordsAsync()
        {
            return _timeSheetRepository.ExportAllRecordsQuery();
        }

        public Task<ICollection<GetTimeSheetDTO>> GetAllSheetsAsync()
        {
            return _timeSheetRepository.GetAllSheetsQuery();
        }

        public Task<GetTimeSheetDTO> GetSheetByIdAndDateAsync(int employeeId, DateOnly workDate)
        {
            return _timeSheetRepository.GetSheetByIdAndDateQuery(employeeId, workDate);
        }

        public Task<ICollection<GetEmployeeSheetDTO>> GetSheetByIdAsync(int id)
        {
            return _timeSheetRepository.GetSheetByIdQuery(id);
        }

        public Task UpdateSheetAsync(int id, TimeSheetDTO timeSheet)
        {
            return _timeSheetRepository.UpdateSheetQuery(id, timeSheet);
        }
    }
}