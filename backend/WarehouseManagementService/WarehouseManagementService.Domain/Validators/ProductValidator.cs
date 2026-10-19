using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WarehouseManagementService.Communication.Dto.Requests;

namespace WarehouseManagementService.Domain.Validators
{
    public class ProductValidator : AbstractValidator<RequestProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatorio");
        }
    }
}