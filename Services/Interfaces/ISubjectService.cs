using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Models;

namespace ANONYMOUS_SURVEY.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<SubjectDto> GetSubjectByIdAsync(int subjectId);
        Task<SubjectWithCommentsDto> GetSubjectWithComments(int subjectId);
        Task<SubjectDto> CreateSubjectAsync(CreateSubjectDto createSubjectDto);
        Task DeleteSubjectAsync(int subjectId);
        Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();

    }
}
