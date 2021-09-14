using Application.Dtos.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.User
{
    public class EditUserProfileDtoRequestValidator : AbstractValidator<EditUserProfileDtoRequest>
    {
        public EditUserProfileDtoRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(255)
                .MinimumLength(5);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.DateOfBirth)
                .NotEmpty();

            RuleFor(x => x.Gender)
                .NotEmpty();
        }
    }
}
