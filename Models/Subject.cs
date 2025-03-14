using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client;

namespace ANONYMOUS_SURVEY.Models
{
    public class Subject
    {
        [Key]
        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Required]
        [MaxLength(256)]
        [Column("subject_name")]
        public string SubjectName { get; set; }

        [Required]
        [Column("department_id")]
        public int DepartmentId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // [ForeignKey("department_id")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}