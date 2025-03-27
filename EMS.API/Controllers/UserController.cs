using System.Collections;
using EMS_Backend_Project.EMS.Application.DTOs.UserDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.UserManagement;
using EMS_Backend_Project.EMS.Common.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend_Project.EMS.API.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<UserDTO>>> GetAll()
        {
            try
            {
                var usersList = await _userService.GetAllUserAsync();

                if (usersList == null)
                    throw new KeyNotFoundException("No User found.");

                return Ok(usersList);
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

        [HttpGet("GetUserByID")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            try
            {
                var users = await _userService.GetUserByIdAsync(id);

                if (users == null)
                    throw new KeyNotFoundException("No User found.");

                return Ok(users);
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

        [HttpPost("Admin")]
        public async Task<ActionResult<string>> AddAdmin(AdminUserDTO adminUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _userService.AddAdminAsync(adminUser);

                return "New Admin Created Successfully. And Credentials will send in Registered Email.";
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

        [HttpPost("Employee")]
        public async Task<ActionResult<string>> AddEmployee(EmplyeeUserDTO emplyeeUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _userService.AddEmployeeAsync(emplyeeUser);

                return "New Employee Created Successfully. And Credentials will send in Registered Email.";
            }
            catch (AlreadyExistsException<string> ex)
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

        [HttpPut("UpdateAdmin")]
        public async Task<ActionResult<string>> UpdateAdmin(int id, AdminUserDTO adminUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _userService.UpdateAdminByIdAsync(id,adminUser);

                return "Admin Updated Successfully.";
            }
            catch (AlreadyExistsException<string> ex)
            {
                return BadRequest(new { Message = ex.Message });
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

        [HttpPut("UpdateEmployee")]
        public async Task<ActionResult<string>> UpdateEmployee(int id, EmplyeeUserDTO emplyeeUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _userService.UpdateEmployeeByIdAsync(id, emplyeeUser);

                return "Employee Updated Successfully.";
            }
            catch(AlreadyExistsException<string> ex)
            {
                return BadRequest(new { Message = ex.Message });
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

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<string>> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserByIdAsync(id);

                return "User Deleted Successfully.";
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