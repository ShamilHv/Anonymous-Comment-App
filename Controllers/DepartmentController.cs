using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANONYMOUS_SURVEY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("department/{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int id)
        {
            try
            {
                var departmentDto = await _departmentService.GetDepartmentByIdAsync(id);
                return Ok(departmentDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("department/{id}/subjects")]
        public async Task<ActionResult<DepartmentWithSubjectsDto>> GetDepartmentWithSubjects(int id)
        {
            try
            {
                var departmentWithSubjectsDto = await _departmentService.GetDepartmentWithSubjectsAsync(id);
                return Ok(departmentWithSubjectsDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("addDepartment")]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            try
            {
                var departmentDto =  await _departmentService.CreateDepartmentAsync(createDepartmentDto);
                return Ok(departmentDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}