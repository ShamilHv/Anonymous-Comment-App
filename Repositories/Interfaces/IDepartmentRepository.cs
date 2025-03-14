using ANONYMOUS_SURVEY.Models;

namespace ANONYMOUS_SURVEY.Repositories.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department> GetDepartmentWithSubjectsAsync(int departmentId);
    }
}