using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANONYMOUS_SURVEY.Models
{
    public class File
    {
        
        [Key]
        [Column("file_id")]
        public int FileId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("file_path")]
        public string FilePath { get; set; }

        [Column("uploaded_at")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}