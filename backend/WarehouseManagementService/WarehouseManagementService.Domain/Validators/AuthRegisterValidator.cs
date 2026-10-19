using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WarehouseManagementService.Communication.Dto.Requests;

namespace WarehouseManagementService.Domain.Validators
{
    public class AuthRegisterValidator : AbstractValidator<RequestAuthRegisterDto>
    {
        public AuthRegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatorio")
                .EmailAddress().WithMessage("Email invalido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatoria")
                .MinimumLength(6).WithMessage("Senha deve ter no minimo 6 caracteres");
            
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatoria")
                .MaximumLength(100);
        }
    }
}