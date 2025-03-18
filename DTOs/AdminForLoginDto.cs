using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ANONYMOUS_SURVEY.DTOs
{
    public partial class AdminForLoginDto
    {
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        [NotNull]
        public String Email { get; set; } = "";
        [Required]
        [MaxLength(255)]
        [NotNull]
        public String Password { get; set; } = "";
    }
}