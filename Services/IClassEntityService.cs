using AttendanceSystem.Domain;
using AttendanceSystem.Error;

namespace AttendanceApp.Services
{
    public interface IClassEntityService
    {
        Task<Result<ClassEntity>> AddClassAsync(ClassEntity entity);
        Task<Result<ClassEntity>> GetClassByIdAsync(int id);
        Task<Result<IEnumerable<ClassEntity>>> GetAllClassesAsync();
        Task<Result<bool>> UpdateClassAsync(ClassEntity entity);
        Task<Result<ClassEntity>> DeleteClassAsync(int id);
    }

}
