namespace ANONYMOUS_SURVEY.DTOs
{
    public class CreateAdminCommentDto
    {
        public string CommentText { get; set; }
        public int ParentCommentId { get; set; }
    }
}