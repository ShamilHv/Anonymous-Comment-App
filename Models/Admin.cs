using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ANONYMOUS_SURVEY.Models
{
    public class Admin
    {
        [Key]
        [Column("admin_id")]
        public int AdminId { get; set; }

        [Required]
        [MaxLength(255)]
        [NotNull]
        [Column("admin_name")]
        public string AdminName { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        [NotNull]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        [NotNull]
        [Column("password")]
        public string PasswordHash { get; set; }=string.Empty;

        [Required]
        [NotNull]
        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } 

        [Column("refresh_token")]
        public string? RefreshToken{get;set;}
        [Column("refresh_token_expiry_time")]
        public DateTime? RefreshTokenExpiryTime{get;set;} 


    }
}