namespace ANONYMOUS_SURVEY.DTOs
{
    public class RefreshTokenRequestDto
    {
        public int AdminId{get;set;}
        public required string RefreshToken{get;set;}
    }
}