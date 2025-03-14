using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANONYMOUS_SURVEY.Models
{
    public class Department
    {
        [Key]
        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("department_name")]
        public string DepartmentName { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}