using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Models;

namespace ANONYMOUS_SURVEY.Repositories.Interfaces
{

    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<Subject> GetSubjectWithCommentsAsync(int subjectId);
        Task <IEnumerable<Subject>> GetSubjectsByDepartment(int departmentId);
        Task  <Subject> GetSubjectByComment(Comment comment);
    }
}