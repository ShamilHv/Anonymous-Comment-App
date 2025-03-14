using ANONYMOUS_SURVEY.Data;
using ANONYMOUS_SURVEY.Models;
using ANONYMOUS_SURVEY.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ANONYMOUS_SURVEY.Repositories
{
    public class CommentRepository : ICommentRepository
    {

        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddAsync(Comment entity)
        {
            await _context.Comments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;

        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Comment>> GetAdminRepliesForCommentAsync(int commentId)
        {
            return await _context.Comments.
            Where(c => c.ParentCommentId == commentId && c.IsAdminComment).
            Include(c => c.Admin).
            ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.File)
                .Include(c => c.Admin)
                .FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsBySubjectAsync(int subjectId)
        {
            return await _context.Comments.
            Where(c => c.SubjectId == subjectId && c.ParentComment == null).
            Include(c => c.Admin).
            Include(c => c.File).
            ToListAsync();
        }

        public async Task<Comment> GetCommentWithRepliesAsync(int commentId)
        {
            return await _context.Comments.
            Include(c => c.File).
            Include(c => c.Subject).
            Include(c => c.Admin).
            Include(c => c.ChildComments).
                ThenInclude(cc => cc.Admin).
                FirstOrDefaultAsync(c => c.CommentId == commentId);
        }

        public async Task UpdateAsync(Comment entity)
        {
            _context.Comments.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}