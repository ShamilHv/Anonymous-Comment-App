namespace ANONYMOUS_SURVEY.DTOs
{
    public class SubjectWithCommentsDto:SubjectDto
    {
        public List<CommentDto> comments{get;set;}=new List<CommentDto>();
    }
}