using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ANONYMOUS_SURVEY.Models
{
    public class Comment
    {
        [Key]
        [Column("comment_id")]
        public int CommentId { get; set; }

        [Required]
        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Required]
        [Column("comment_text", TypeName = "text")]
        public string CommentText { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("parent_comment_id")]
        public int? ParentCommentId { get; set; }

        [Column("file_id")]
        public int? FileId { get; set; }

        [Column("is_admin_comment")]
        public bool IsAdminComment { get; set; } = false;

        [Column("admin_id")]
        public int? AdminId { get; set; }

       // [ForeignKey("subject_id")]
        public virtual Subject? Subject { get; set; }

        //[ForeignKey("parent_comment_id")]
        public virtual Comment ParentComment { get; set; }
        
        public virtual ICollection<Comment> ChildComments { get; set; } = new List<Comment>();

       // [ForeignKey("file_id")]
        public virtual File? File { get; set; }

       // [ForeignKey("admin_id")]
        public virtual Admin Admin { get; set; }


    }
}