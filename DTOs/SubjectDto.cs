using Microsoft.VisualBasic;

namespace ANONYMOUS_SURVEY.DTOs
{
    public class SubjectDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreatedAt{get;set;}

    }
}