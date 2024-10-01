using AttendanceSystem.Domain;
using FluentValidation;

namespace AttendanceSystem.Validators
{
    public class ClassEntityValidator : AbstractValidator<ClassEntity>
    {
        public ClassEntityValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Class name is required.");
        }
    }

}
