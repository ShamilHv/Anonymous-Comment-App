namespace ANONYMOUS_SURVEY.DTOs
{
    public class AdminDto
    {

        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public string Email { get; set; }
        public int SubjectId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}