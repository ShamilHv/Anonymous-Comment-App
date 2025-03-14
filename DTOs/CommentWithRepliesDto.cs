namespace ANONYMOUS_SURVEY.DTOs
{
    public class CommentWithRepliesDto:CommentDto
    {
        public List<CommentDto> Replies { get; set; } = new List<CommentDto>();

    }
}