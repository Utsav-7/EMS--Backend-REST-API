using EMS_Backend_Project.EMS.Application.DTOs.TimeSheetDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend_Project.EMS.Application.Interfaces.TimeSheetManagement
{
    public interface ITimeSheetService
    {
        Task<ICollection<GetTimeSheetDTO>> GetAllSheetsAsync();
        Task<GetTimeSheetDTO> GetSheetByIdAndDateAsync(int employeeId, DateOnly workDate);
        Task AddSheetAsync(int employeeId, TimeSheetDTO timeSheet);
        Task UpdateSheetAsync(int id, TimeSheetDTO timeSheet);
        Task DeleteSheetAsync(int id, DateOnly workDate);
        Task<ICollection<GetEmployeeSheetDTO>> GetSheetByIdAsync(int id);
        Task<FileContentResult> ExportAllRecordsAsync();
    }
}
