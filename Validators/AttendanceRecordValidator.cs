using AttendanceSystem.Domain;
using FluentValidation;

namespace AttendanceSystem.Validators
{
    public class AttendanceRecordValidator : AbstractValidator<AttendanceRecord>
    {
        public AttendanceRecordValidator()
        {
            RuleFor(ar => ar.StudentId).NotEmpty().WithMessage("Student is required.");
            RuleFor(ar => ar.ClassSessionId).NotEmpty().WithMessage("Class session is required.");
        }
    }

}
