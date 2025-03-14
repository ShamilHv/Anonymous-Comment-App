using System.ComponentModel.DataAnnotations.Schema;

namespace ANONYMOUS_SURVEY.DTOs
{
    public class CreateCommentDto
    {
        [Column("subject_id")]
        public int SubjectId { get; set; }
        public string CommentText { get; set; }
        public int? ParentCommentId { get; set; }=null;
        public IFormFile? File { get; set;}
    }
}