using AttendanceApp.Repositories;
using AttendanceSystem.Domain;
using AttendanceSystem.Error;
using AttendanceSystem.Validators;

namespace AttendanceApp.Services
{
    public class ClassEntityService : IClassEntityService
    {
        private readonly IClassEntityRepository _repository;
        private readonly ClassEntityValidator _validator;
        public ClassEntityService(IClassEntityRepository repository, ClassEntityValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<Result<ClassEntity>> AddClassAsync(ClassEntity entity)
        {
           var validationResult = _validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                return Result<ClassEntity>.Failure(validationResult.ToString());
            }
            var isAdded = await _repository.AddAsync(entity);
            if (!isAdded.IsSuccess)
            {
                return Result<ClassEntity>.Failure("Failed to add class");
            }
            return Result<ClassEntity>.Success(isAdded.Output);
        }

        public async Task<Result<ClassEntity>> DeleteClassAsync(int id)
        {
            var existingClass = await _repository.GetByIdAsync(id);
            if (existingClass == null)
            {
                return Result<ClassEntity>.Failure("Class not found");
            }

            var isDeleted = await _repository.DeleteAsync(id);
            if (!isDeleted.IsSuccess)
            {
                return Result<ClassEntity>.Failure("Failed to delete the class.");
            }

            return Result<ClassEntity>.Success(existingClass.Output);
        }


        public async Task<Result<IEnumerable<ClassEntity>>> GetAllClassesAsync()
        {
            var classes = await _repository.GetAllAsync();
            if (!classes.IsSuccess)
            {
                return Result<IEnumerable<ClassEntity>>.Failure("Failed to retrieve classes.");
            }
            return Result<IEnumerable<ClassEntity>>.Success(classes.Output);
        }

        public async Task<Result<ClassEntity>> GetClassByIdAsync(int id)
        {
            var classEntity = await _repository.GetByIdAsync(id);
            if (classEntity == null)
            {
                return Result<ClassEntity>.Failure(classEntity.ErrorMessage);
            }
            return Result<ClassEntity>.Success(classEntity.Output);
        }

        public async Task<Result<bool>> UpdateClassAsync(ClassEntity entity)
        {
            var validationResult = _validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                return Result<bool>.Failure(validationResult.ToString());
            }
            var isUpdated = await _repository.UpdateAsync(entity);
            if (!isUpdated.IsSuccess)
            {
                return Result<bool>.Failure("Failed to update the class.");
            }
            return Result<bool>.Success(true);
        }
    }
}
