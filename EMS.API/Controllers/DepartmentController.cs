using EMS_Backend_Project.EMS.Application.DTOs.DepartmentDTOs;
using EMS_Backend_Project.EMS.Application.Interfaces.DepartmentManagement;
using EMS_Backend_Project.EMS.Common.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend_Project.EMS.API.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ICollection<GetDepartmentDTO>>> GetAll()
        {
            try
            {
                var departmentList = await _departmentService.GetAllDepartmentAsync();
                return Ok(departmentList);
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

        [HttpGet("GetById")]
        public async Task<ActionResult<GetDepartmentDTO>> GetById(int empId)
        {
            try
            {
                var department = await _departmentService.GetDepartmentByIdAsync(empId);
                return Ok(department);
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
        public async Task<ActionResult> Add(string deptName)
        {
            try
            {
                await _departmentService.AddDepartmentAsync(deptName);
                return Ok("Department Created Successfully.");
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

        [HttpPut]
        public async Task<ActionResult> Update(int deptId, string name)
        {
            try
            {
                await _departmentService.UpdateDepartmentAsync(deptId, name);
                return Ok("Department Updated Successfully.");
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
        public async Task<ActionResult> Deletee(int deptId)
        {
            try
            {
                await _departmentService.DeleteDepartmentAsync(deptId);
                return Ok("Department Deleted Successfully.");
            }
            catch (DataNotFoundException<int> ex)
            {
                return NotFound(new { Message = ex.Message.ToString() });
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