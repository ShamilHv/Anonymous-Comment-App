namespace ANONYMOUS_SURVEY.DTOs
{
        public class CreateAdminCommentDto
    {
        public int SubjectId { get; set; }
        public string CommentText { get; set; }
        public int ParentCommentId { get; set; }
    }
}