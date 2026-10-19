using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WarehouseManagementService.Communication.Dto.Requests;

namespace WarehouseManagementService.Domain.Validators
{
    public class OrderValidator  : AbstractValidator<RequestOrderDto>
    {
        public OrderValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Descricao é obrigatoria");

            RuleFor(x => x.Tasks)
                .NotEmpty().WithMessage("É preciso de pelo menos uma tarefa para criar um pedido");
        }
    }
}