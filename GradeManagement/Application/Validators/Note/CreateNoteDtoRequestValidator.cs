using Application.Dtos.Notes;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Validators.Note
{
    public class CreateNoteDtoRequestValidator : AbstractValidator<CreateNoteDtoRequest>
    {
        public CreateNoteDtoRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Payload)
                .NotEmpty();

            RuleFor(x => x.CreatedAt)
                .NotEmpty();
        }
    }
}
