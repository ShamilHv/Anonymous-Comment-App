using System.Text.Json.Serialization;
using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ANONYMOUS_SURVEY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("comments/{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                return Ok(comment);
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
        [HttpGet("comments/{id}/replies")]
        public async Task<ActionResult<CommentWithRepliesDto>> GetCommentWithReplies(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentWithRepliesAsync(id);
                return Ok(comment);
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
        [HttpGet("subjects/{subjectId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsBySubject(int subjectId)
        {
            try
            {
                var comments = await _commentService.GetCommentsBySubjectAsync(subjectId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("anonymous")]
        public async Task<ActionResult<CommentDto>> CreateAnonymousComment(CreateCommentDto createCommentDto)
        {
            try
            {
                var comment = await _commentService.CreateAnonymousCommentAsync(createCommentDto);
                return CreatedAtAction(nameof(GetComment), new { id = comment.CommentId }, comment);
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

        // [HttpPost("admin")]
        // public async Task<ActionResult<CommentDto>> CreateAdminComment(CreateAdminCommentDto createAdminCommentDto)
        // {
        //     try
        //     {
        //         var comment = await _commentService.CreateAdminCommentAsync(createAdminCommentDto, adminId);
        //         return CreatedAtAction(nameof(GetComment), new { id = comment.CommentId }, comment);
        //     }
        //     catch (KeyNotFoundException ex)
        //     {
        //         return NotFound(ex.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }


    }
}