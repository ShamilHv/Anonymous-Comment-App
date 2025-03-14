using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Repositories.Interfaces;
using ANONYMOUS_SURVEY.Services;
using ANONYMOUS_SURVEY.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ANONYMOUS_SURVEY.Controllers
{
[ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet("subject/{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubjectById(int id)
        {
            try
            {
                var subjectDto = await _subjectService.GetSubjectByIdAsync(id);
                return Ok(subjectDto);
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
        [HttpGet("subject/{id}/comments")]
        public async Task<ActionResult<SubjectWithCommentsDto>> GetSubjectWithComments(int id)
        {
            try
            {
                var subjectWithCommentsDto = await _subjectService.GetSubjectWithComments(id);
                return Ok(subjectWithCommentsDto);
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
        [HttpPost("addSubject")]
        public async Task<ActionResult<SubjectDto>> CreateSubject(CreateSubjectDto createSubjectDto)
        {
            try
            {
                var departmentDto =  await _subjectService.CreateSubjectAsync(createSubjectDto);
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