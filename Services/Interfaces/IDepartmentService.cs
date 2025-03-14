using ANONYMOUS_SURVEY.DTOs;

namespace ANONYMOUS_SURVEY.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId);
        Task<DepartmentWithSubjectsDto> GetDepartmentWithSubjectsAsync(int departmentId);
        Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto);
    }
}