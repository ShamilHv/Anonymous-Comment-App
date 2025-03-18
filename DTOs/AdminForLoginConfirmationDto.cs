namespace ANONYMOUS_SURVEY.DTOs
{
    public partial class AdminForLoginConfirmationDto
    {
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];
    }
}