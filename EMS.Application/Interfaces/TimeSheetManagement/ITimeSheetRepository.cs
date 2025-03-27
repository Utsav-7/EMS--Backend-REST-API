using EMS_Backend_Project.EMS.Application.DTOs.TimeSheetDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend_Project.EMS.Application.Interfaces.TimeSheetManagement
{
    public interface ITimeSheetRepository
    {
        Task<ICollection<GetTimeSheetDTO>> GetAllSheetsQuery();
        Task<GetTimeSheetDTO> GetSheetByIdAndDateQuery(int employeeId, DateOnly workDate);
        Task AddSheetQuery(int employeeId,TimeSheetDTO timeSheet);
        Task UpdateSheetQuery(int id, TimeSheetDTO timeSheet);
        Task DeleteSheetQuery(int id, DateOnly workDate);
        Task<ICollection<GetEmployeeSheetDTO>> GetSheetByIdQuery(int id);
        Task<FileContentResult> ExportAllRecordsQuery();
        Task<FileContentResult> ExportAllRecordsByIdQuery(int empId);
    }
}