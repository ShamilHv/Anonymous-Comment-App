using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Models;
using ANONYMOUS_SURVEY.Repositories.Interfaces;
using ANONYMOUS_SURVEY.Services.Interfaces;

namespace ANONYMOUS_SURVEY.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentrepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentrepository = departmentRepository;
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
        {
            var allDepartments = await _departmentrepository.GetAllAsync();
            foreach (Department d in allDepartments)
            {
                if (d.DepartmentName == createDepartmentDto.DepartmentName)
                {
                    throw new Exception("Department with the name " + createDepartmentDto.DepartmentName + " already exists");
                }
            }

            var department = new Department
            {
                DepartmentName = createDepartmentDto.DepartmentName,
                CreatedAt = DateTime.UtcNow
            };

            var addedDepartment = await _departmentrepository.AddAsync(department);
            return MapToDepartmentDto(addedDepartment);
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId)
        {
            var department = await _departmentrepository.GetByIdAsync(departmentId);
            if (department == null)
            {
                throw new KeyNotFoundException("Department with the id " + departmentId + " not found");
            }
            return MapToDepartmentDto(department);
        }

        public async Task<DepartmentWithSubjectsDto> GetDepartmentWithSubjectsAsync(int departmentId)
        {
            var department = await _departmentrepository.GetDepartmentWithSubjectsAsync(departmentId);
            if (department == null)
            {
                throw new KeyNotFoundException("Department with the id " + departmentId + " not found");
            }
            var result = new DepartmentWithSubjectsDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                subjects = department.Subjects.Select(MapToSubjectDto).ToList()
            };
            return result;
        }
        private DepartmentDto MapToDepartmentDto(Department department)
        {
            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                CreatedAt = department.CreatedAt
            };
        }
        private SubjectDto MapToSubjectDto(Subject subject)
        {
            return new SubjectDto
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                CreatedAt = subject.CreatedAt,
                DepartmentId = subject.DepartmentId
            };
        }
    }
}