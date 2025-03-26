using System.Security.Claims;
using EMS_Backend_Project.EMS.Application.DTOs.EmployeeDTOs;
using EMS_Backend_Project.EMS.Application.DTOs.LeavesDTOs;
using EMS_Backend_Project.EMS.Application.DTOs.TimeSheetDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.EmployeeDashboard;
using EMS_Backend_Project.EMS.Application.Interfaces.LeaveManagement;
using EMS_Backend_Project.EMS.Application.Interfaces.TimeSheetManagement;
using EMS_Backend_Project.EMS.Common.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend_Project.EMS.API.Controllers
{
    [Authorize(Roles = "Employee")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly ITimeSheetService _timeSheetService;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILeaveService leaveService, ITimeSheetService timeSheetService, IEmployeeService employeeService)
        {
            _leaveService = leaveService;
            _timeSheetService = timeSheetService;
            _employeeService = employeeService;
        }

        // Extract the logged-in user's ID from the JWT token   
        private int GetLoggedInUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        [HttpGet("Profile")]
        public async Task<ActionResult> Profile()
        {
            try
            {
                var loginUser = GetLoggedInUserId();
                var profileData = await _employeeService.GetProfileDataAsync(loginUser);

                return Ok(profileData);
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

        [HttpPut("UpdateProfile")]
        public async Task<ActionResult> UpdateProfile(EmployeeUpdateDTO employeeUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                var loggedUser = GetLoggedInUserId();
                await _employeeService.UpdateProfileAsync(loggedUser, employeeUpdate);

                return Ok("Your Data has been updated.");
            }
            catch(DataNotFoundException<string> ex)
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

        [HttpGet("YourLeaves")]
        public async Task<ActionResult> GetById()
        {
            try
            {
                var loggedUser = GetLoggedInUserId();
                var leaveRecord = await _leaveService.GetLeaveByIDAsync(loggedUser);

                return Ok(leaveRecord);
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

        [HttpPost("TakeLeave")]
        public async Task<ActionResult> Add(LeaveDTO leave)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var employeeId = GetLoggedInUserId();
                await _leaveService.AddLeaveAsync(employeeId, leave);

                return Ok("Leave record created Successfully.");
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

        [HttpGet("GetYourSheets")]
        public async Task<ActionResult<TimeSheetDTO>> GetYourRecords()
        {
            try
            {
                int currentUser = GetLoggedInUserId();
                var sheetList = await _timeSheetService.GetSheetByIdAsync(currentUser);

                return Ok(sheetList);
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

        [HttpPost("CreateTimeSheet")]
        public async Task<ActionResult> AddTimeSheet(TimeSheetDTO timeSheet)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var employeeId = GetLoggedInUserId();
                await _timeSheetService.AddSheetAsync(employeeId, timeSheet);

                return Ok("Time Sheet Created Successfully.");
            }
            catch (AlreadyExistsException<string> ex)
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

        [HttpPut("UpdateTimeSheet")]
        public async Task<ActionResult> Update(TimeSheetDTO timeSheet)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var loggedUser = GetLoggedInUserId();
                await _timeSheetService.UpdateSheetAsync(loggedUser, timeSheet);

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

        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword(EmployeePwdUpdateDTO employeePwdUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (employeePwdUpdate.NewPassword != employeePwdUpdate.ConfirmPassword)
                return BadRequest("New Password and Confirm Password is not matching.");

            try
            {
                var loggedUser = GetLoggedInUserId();
                await _employeeService.ChangePasswordAsync(loggedUser, employeePwdUpdate);

                return Ok("Your Password will be updated Successfully.");
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
    }
}