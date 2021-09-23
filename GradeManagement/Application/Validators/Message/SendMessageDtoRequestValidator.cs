using Application.Dtos.Message;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Message
{
    public class SendMessageDtoRequestValidator : AbstractValidator<SendMessageDtoRequest>
    {
        public SendMessageDtoRequestValidator()
        {
            RuleFor(x => x.ReceiverId)
                .NotEmpty();
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(255);
            RuleFor(x => x.PostedOn)
                .NotEmpty();
        }
    }
}
