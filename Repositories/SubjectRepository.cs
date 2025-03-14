using ANONYMOUS_SURVEY.Data;
using ANONYMOUS_SURVEY.Models;
using ANONYMOUS_SURVEY.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ANONYMOUS_SURVEY.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;
        public SubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Subject> AddAsync(Subject entity)
        {
            await _context.Subjects.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            return await _context.Subjects.ToListAsync();
        }

        public async Task<Subject> GetByIdAsync(int id)
        {
            return await _context.Subjects.FindAsync(id);
        }

        public async Task<IEnumerable<Subject>> GetSubjectsByDepartment(int departmentId)
        {
            return await _context.Subjects.
            Where(s => s.DepartmentId == departmentId).
            Include(s => s.Admins).
            Include(s => s.Department).
            ToListAsync();

        }

        public async Task<Subject> GetSubjectWithCommentsAsync(int subjectId)
        {
            return await _context.Subjects
               .Where(s => s.SubjectId == subjectId)
               .Include(s => s.Comments)
               .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Subject entity)
        {
            _context.Subjects.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}