using ANONYMOUS_SURVEY.Data;
using ANONYMOUS_SURVEY.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ANONYMOUS_SURVEY.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;

        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Models.File> AddAsync(Models.File entity)
        {
            await _context.Files.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file != null)
            {
                _context.Files.Remove(file);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Models.File>> GetAllAsync()
        {
            return await _context.Files.ToListAsync();
        }

        public async Task<Models.File> GetByIdAsync(int id)
        {
            return await _context.Files.FindAsync(id);
        }

        public async Task UpdateAsync(Models.File entity)
        {
             _context.Files.Update(entity);
             await _context.SaveChangesAsync();
        }
    }
}