using ANONYMOUS_SURVEY.Models;
using Microsoft.EntityFrameworkCore;

namespace ANONYMOUS_SURVEY.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Comment> Auth { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Subject>().ToTable("Subjects");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Models.File>().ToTable("Files");
            modelBuilder.Entity<Comment>().ToTable("Comments");

            modelBuilder.Entity<Subject>()
            .HasOne(s => s.Department)
            .WithMany(d => d.Subjects)
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<Admin>()
            //    .HasOne(a => a.Subject)
            //    .WithMany(s => s.Admins)
            //    .HasForeignKey(a => a.SubjectId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Subject)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.ChildComments)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.File)
                .WithMany(f => f.Comments)
                .HasForeignKey(c => c.FileId)
                .OnDelete(DeleteBehavior.SetNull);

            // modelBuilder.Entity<Comment>()
            //     .HasOne(c => c.Admin)
            //     .WithMany(a => a.AdminComments)
            //     .HasForeignKey(c => c.AdminId)
            //     .OnDelete(DeleteBehavior.SetNull);
        }

    }
}