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
                .NotEmpty().WithMessage("É preciso de pelo menos uma tarefa para criar um pedido")
                .Custom((tasks, context) =>
                    {
                        tasks.GroupBy(t => t.ProductId)
                            .Where(g => g.Count() > 1)
                            .ToList()
                            .ForEach(g =>
                            {
                                context.AddFailure($"Produto {g.Key} está duplicado nas tarefas");
                            });
                    });
        }
    }
}