using ANONYMOUS_SURVEY.Models;

namespace ANONYMOUS_SURVEY.Repositories.Interfaces
{
    public interface IAdminRepository: IRepository<Admin>
    {
        Task<Admin> GetByEmailAsync(string email);
    }
}