using System.Security.Claims;
using EMS_Backend_Project.EMS.Application.DTOs.TimeSheetDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.TimeSheetManagement;
using EMS_Backend_Project.EMS.Common.CustomExceptions;
using EMS_Backend_Project.EMS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend_Project.EMS.API.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetController : ControllerBase
    {
        private readonly ITimeSheetService _timeSheetService;
        public TimeSheetController(ITimeSheetService timeSheetService)
        {
            _timeSheetService = timeSheetService;
        }

        // Extract the logged-in user's ID from the JWT token   
        private int GetLoggedInUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        [HttpGet("GetAllSheet")]
        public async Task<ActionResult<GetTimeSheetDTO>> GetAll()
        {
            try
            {
                var list = await _timeSheetService.GetAllSheetsAsync();

                return Ok(list);
            }
            catch (DataNotFoundException<string> ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An unexpected error occurred. : {ex.Message}" });
            }
        }

        [HttpGet("GetSheetByID&Date")]
        public async Task<ActionResult<GetTimeSheetDTO>> GetByIdDate(int id, DateOnly date)
        {
            if (id <= 0)
                return BadRequest("Invalid ID. It must be a positive number.");

            try
            {
                var sheet = await _timeSheetService.GetSheetByIdAndDateAsync(id, date);

                return Ok(sheet);
            }
            catch (DataNotFoundException<int> ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An unexpected error occurred. : {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(TimeSheetDTO timeSheet)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var loggedUserId = GetLoggedInUserId();
                await _timeSheetService.AddSheetAsync(loggedUserId, timeSheet);

                return Ok("Time Sheet Created Successfully.");
            }
            catch(AlreadyExistsException<string> ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An unexpected error occurred. : {ex.Message}" });
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(int employeeId, TimeSheetDTO timeSheet)
        {
            if (employeeId <= 0)
                return BadRequest("Invalid ID. It must be a positive number.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _timeSheetService.UpdateSheetAsync(employeeId, timeSheet);

                return Ok("Time Sheet Updated Successfully.");
            }
            catch (DataNotFoundException<int> ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An unexpected error occurred. : {ex.Message}" });
            }
        }

        [HttpDelete]
        public async Task<ActionResult<GetTimeSheetDTO>> Delete(int id, DateOnly date)
        {
            if (id <= 0)
                return BadRequest("Invalid ID. It must be a positive number.");

            try
            {
                await _timeSheetService.DeleteSheetAsync(id, date);

                return Ok("Time Sheet Deleted Successfully.");
            }
            catch (DataNotFoundException<int> ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An unexpected error occurred. : {ex.Message}" });
            }
        }

        [HttpGet("GenerateExcel")]
        public async Task<ActionResult<TimeSheetDTO>> DownloadExcel()
        {
            try
            {
                var sheetList = await _timeSheetService.ExportAllRecordsAsync();

                return sheetList;
            }
            catch (DataNotFoundException<string> ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An unexpected error occurred. : {ex.Message}" });
            }
        }


        [HttpGet("GenerateExcelByEmpID")]
        public async Task<ActionResult<TimeSheetDTO>> DownloadExcelById(int employeeId)
        {
            try
            {
                var sheetList = await _timeSheetService.ExportAllRecordsByIdAsync(employeeId);

                return sheetList;
            }
            catch (DataNotFoundException<string> ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An unexpected error occurred. : {ex.Message}" });
            }
        }

    }
}