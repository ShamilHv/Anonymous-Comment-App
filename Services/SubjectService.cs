using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Models;
using ANONYMOUS_SURVEY.Repositories.Interfaces;
using ANONYMOUS_SURVEY.Services.Interfaces;

namespace ANONYMOUS_SURVEY.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public SubjectService(ISubjectRepository subjectRepository, IDepartmentRepository departmentRepository)
        {
            _subjectRepository = subjectRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task DeleteSubjectAsync(int subjectId)
        {
            await _departmentRepository.DeleteAsync(subjectId);
        }

        public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
        {
            var subjects=await _subjectRepository.GetAllAsync();
            if(!subjects.Any()){
                throw new Exception("No Subjects as of now");
            }
            return subjects.Select(MapToSubjectDto);
        }
        public async Task<SubjectDto> CreateSubjectAsync(CreateSubjectDto createSubjectDto)
        {
            var allSubjects = await _subjectRepository.GetAllAsync();
            foreach (Subject d in allSubjects)
            {
                if (d.SubjectName == createSubjectDto.SubjectName)
                {
                    throw new Exception("Subject with the name " + createSubjectDto.SubjectName + " already exists");
                }
            }
            var department = await _departmentRepository.GetByIdAsync(createSubjectDto.DepartmentId);
            if (department == null)
            {
                throw new KeyNotFoundException("Subject with the id " + createSubjectDto.DepartmentId + " deos not exist");
            }

            var subject = new Subject
            {
                SubjectName = createSubjectDto.SubjectName,
                CreatedAt = DateTime.UtcNow,
                DepartmentId = createSubjectDto.DepartmentId
            };
            var addedSubject = await _subjectRepository.AddAsync(subject);
            return MapToSubjectDto(addedSubject);

        }

        public async Task<SubjectDto> GetSubjectByIdAsync(int subjectId)
        {
            var subject = await _subjectRepository.GetByIdAsync(subjectId);
            if (subject == null)
            {
                throw new KeyNotFoundException("Subject with the id " + subjectId + " not found");
            }
            return MapToSubjectDto(subject);
        }

        public async Task<SubjectWithCommentsDto> GetSubjectWithComments(int subjectId)
        {
            var subject = await _subjectRepository.GetSubjectWithCommentsAsync(subjectId);
            if (subject == null)
            {
                throw new KeyNotFoundException("Subject with the id " + subjectId + " not found");
            }

            var result = new SubjectWithCommentsDto
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                CreatedAt = subject.CreatedAt,
                DepartmentId = subject.DepartmentId,
                comments = subject.Comments?.Select(MapToCommentDto).ToList() ?? new List<CommentDto>()
            };
            return result;
        }
        private SubjectDto MapToSubjectDto(Subject subject)
        {
            return new SubjectDto
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                CreatedAt = subject.CreatedAt,
                DepartmentId = subject.DepartmentId
            };
        }
        private CommentDto MapToCommentDto(Comment comment)
        {
            return new CommentDto
            {
                CommentId = comment.CommentId,
                SubjectId = comment.SubjectId,
                CommentText = comment.CommentText,
                CreatedAt = comment.CreatedAt,
                ParentCommentId = comment.ParentCommentId,
                HasFile = comment.FileId.HasValue,
                FilePath = comment.File?.FilePath,
                IsAdminComment = comment.IsAdminComment,
                // AdminName = comment.Admin?.AdminName
            };
        }


    }
}