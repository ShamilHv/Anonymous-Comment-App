using ANONYMOUS_SURVEY.Data;
using ANONYMOUS_SURVEY.Models;
using ANONYMOUS_SURVEY.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ANONYMOUS_SURVEY.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Department> AddAsync(Department entity)
        {
            await _context.Departments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _context.Departments.Where(d=>d.DepartmentId==id).
            FirstOrDefaultAsync();
        }

        public async Task<Department> GetDepartmentWithSubjectsAsync(int departmentId)
        {
            return await _context.Departments.Where(d=>d.DepartmentId==departmentId).
            Include(d=>d.Subjects).
            FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Department entity)
        {
            _context.Departments.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}