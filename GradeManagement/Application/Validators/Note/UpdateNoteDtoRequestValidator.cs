using Application.Dtos.Notes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Note
{
    public class UpdateNoteDtoRequestValidator : AbstractValidator<UpdateNoteDtoRequest>
    {
        public UpdateNoteDtoRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Payload)
                .NotEmpty();
        }
    }
}
