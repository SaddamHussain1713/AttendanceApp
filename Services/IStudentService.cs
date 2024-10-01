using AttendanceSystem.Domain;
using AttendanceSystem.Error;
using AttendanceSystem.Repositories;
using AttendanceSystem.Validators;

namespace AttendanceApp.Services
{
    public interface IStudentService
    {
        Task<Result<Student>> AddStudentAsync(Student student);
        Task<Result<Student>> GetStudentByIdAsync(int id);
        Task<Result<IEnumerable<Student>>> GetAllStudentsAsync();
        Task<Result<bool>> UpdateStudentAsync(Student student);
        Task<Result<Student>> DeleteStudentAsync(int id);

    }
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly StudentValidator _validator;

        public StudentService(IStudentRepository studentRepository, StudentValidator validator)
        {
            _studentRepository = studentRepository;
            _validator = validator;
        }

        public async Task<Result<Student>> DeleteStudentAsync(int id)
        {
            var result = await _studentRepository.DeleteAsync(id);
            if(result == null)
            {
                return Result<Student>.Failure("Student not found");
            }
            return Result<Student>.Success(result.Output);
        }

        public async Task<Result<IEnumerable<Student>>> GetAllStudentsAsync()
        {
            var result = await _studentRepository.GetAllAsync();
            if(result == null)
            {
                return Result<IEnumerable<Student>>.Failure("No students found");
            }
            return Result<IEnumerable<Student>>.Success(result.Output);
        }
        public async Task<Result<Student>> GetStudentByIdAsync(int id)
        {
            var result = await _studentRepository.GetByIdAsync(id);
            if(result == null)
            {
                return Result<Student>.Failure("Student not found");
            }
            return Result<Student>.Success(result.Output);
        }

        public async Task<Result<Student>> AddStudentAsync(Student student)
        {
            var result = _validator.Validate(student);
            if(!result.IsValid)
            {
                return Result<Student>.Failure(result.ToString());
            }
            var isAdded = await _studentRepository.AddAsync(student);
            if(isAdded == null)
            {
                return Result<Student>.Failure("Student not added");
            }
            return Result<Student>.Success(isAdded.Output);
        }

        public async Task<Result<bool>> UpdateStudentAsync(Student student)
        {
            var result = _validator.Validate(student);
            if(!result.IsValid)
            {
                return Result<bool>.Failure(result.ToString());
            }
            var isUpdated = await _studentRepository.UpdateAsync(student);
            if(!isUpdated.IsSuccess)
            {
                return Result<bool>.Failure("Student not updated");
            }
            return Result<bool>.Success(isUpdated.IsSuccess);
        }
    }
}
