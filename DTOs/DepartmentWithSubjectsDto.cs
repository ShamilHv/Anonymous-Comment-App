using ANONYMOUS_SURVEY.Models;
namespace ANONYMOUS_SURVEY.DTOs
{
    public class DepartmentWithSubjectsDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime CreatedAt{get;set;}
        public List<SubjectDto> subjects{get;set;} = new List<SubjectDto>();

    }
}