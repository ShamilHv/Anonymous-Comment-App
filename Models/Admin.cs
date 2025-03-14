using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANONYMOUS_SURVEY.Models
{
    public class Admin
    {
        [Key]
        [Column("admin_id")]
        public int AdminId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("admin_name")]
        public string AdminName { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Subject Subject { get; set; }

        public virtual ICollection<Comment> AdminComments { get; set; } = new List<Comment>();
    }
}