using ANONYMOUS_SURVEY.Models;

namespace ANONYMOUS_SURVEY.Repositories.Interfaces
{
    public interface ICommentRepository: IRepository<Comment>
    {
        Task<Comment> GetCommentWithRepliesAsync(int commentId);
        Task<IEnumerable<Comment>> GetCommentsBySubjectAsync(int subjectId);
        Task <IEnumerable<Comment>> GetAdminRepliesForCommentAsync(int commentId);
    }

}