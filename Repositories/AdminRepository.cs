using System.Formats.Asn1;
using ANONYMOUS_SURVEY.Data;
using ANONYMOUS_SURVEY.Models;
using ANONYMOUS_SURVEY.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ANONYMOUS_SURVEY.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        public AdminRepository(ApplicationDbContext context){
            _context=context;
        }
        public async Task<Admin> AddAsync(Admin entity)
        {
            await _context.Admins.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var admin=await _context.Admins.FindAsync(id);
            if(admin!=null){
                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            return await _context.Admins.ToListAsync();
        }

        public async Task<Admin> GetByEmailAsync(string email)
        {
            return await _context.Admins.Where(a=>a.Email==email)
            .FirstOrDefaultAsync();
        }

        public async Task<Admin> GetByIdAsync(int id)
        {
            return await _context.Admins.FindAsync(id);
        }

        public async Task UpdateAsync(Admin entity)
        {
            _context.Admins.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}