using AttendanceSystem.Domain;
using FluentValidation;

namespace AttendanceSystem.Validators
{
    public class ClassSessionValidator : AbstractValidator<ClassSession>
    {
        public ClassSessionValidator()
        {
            RuleFor(cs => cs.SessionDate).NotEmpty().WithMessage("Session date is required.");
            RuleFor(cs => cs.ClassId).NotEmpty().WithMessage("Class is required.");
        }
    }

}
