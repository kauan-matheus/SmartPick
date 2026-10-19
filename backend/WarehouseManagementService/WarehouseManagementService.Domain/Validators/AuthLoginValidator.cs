using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WarehouseManagementService.Communication.Dto.Requests;

namespace WarehouseManagementService.Domain.Validators
{
    public class AuthLoginValidator : AbstractValidator<RequestAuthLoginDto>
    {
        public AuthLoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatorio")
                .EmailAddress().WithMessage("Email invalido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatoria");
        }
    }
}