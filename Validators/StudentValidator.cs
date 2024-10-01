using AttendanceSystem.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Validators
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(s => s.FirstName).NotEmpty().WithMessage("First name is required.")
                .Length(4, 20).WithMessage("First name must be between 4 and 20 characters."); ;
            RuleFor(s => s.LastName).NotEmpty().WithMessage("Last name is required.")
                .Length(4, 20).WithMessage("First name must be between 4 and 20 characters.");
            RuleFor(s => s.Email).NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid Email address");
            RuleFor(s => s.Phone).NotEmpty().WithMessage("Phone number is required.");
            RuleFor(s => s.ClassId).NotEmpty().WithMessage("Class is required.");
        }
    }

}
