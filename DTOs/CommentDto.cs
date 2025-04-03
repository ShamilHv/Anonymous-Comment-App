using System.ComponentModel.DataAnnotations.Schema;

namespace ANONYMOUS_SURVEY.DTOs
{
    public class CommentDto{
        public int CommentId { get; set; }
        public int SubjectId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ParentCommentId { get; set; }
        public bool HasFile { get; set; }
        public string? FilePath { get; set; }
        public bool IsAdminComment { get; set; }
    }
}