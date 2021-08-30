using Application.Dtos.User;
using Domain.Models;
using FluentValidation;
using Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class RegisterUserDtoRequestValidator : AbstractValidator<RegisterUserDtoRequest>
    {
        private string[] allowedRoleNames = new string[] { 
            Role.Administrator,
            Role.Teacher,
            Role.Student 
        };

        public RegisterUserDtoRequestValidator()
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

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(255)
                .MinimumLength(5);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.Role)
                .NotEmpty()
                .Must(x => allowedRoleNames.Contains(x))
                .WithMessage($"Role must be in [{string.Join(",", allowedRoleNames)}]");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty();
        }
    }
}
