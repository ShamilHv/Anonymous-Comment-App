using ANONYMOUS_SURVEY.DTOs;

namespace ANONYMOUS_SURVEY.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> GetCommentByIdAsync(int commentId);
        Task<CommentWithRepliesDto> GetCommentWithRepliesAsync(int commentId);
        Task<IEnumerable<CommentDto>> GetCommentsBySubjectAsync(int subjectId);
        Task<CommentDto> CreateAnonymousCommentAsync(CreateCommentDto createCommentDto);
        Task<CommentDto> CreateAdminCommentAsync(CreateAdminCommentDto createAdminCommentDto);
 
    }
}