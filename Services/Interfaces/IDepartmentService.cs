using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Models;

namespace ANONYMOUS_SURVEY.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId);
        Task<DepartmentWithSubjectsDto> GetDepartmentWithSubjectsAsync(int departmentId);
        Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto);
        Task DeleteDepartmentAsync(int departmentId);
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
    }
}