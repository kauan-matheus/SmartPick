using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WarehouseManagementService.Communication.Dto.Responses;

namespace WarehouseManagementService.Api.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = ServiceResponse<object>.ValidationError(errors);

                context.Result = new BadRequestObjectResult(response.Errors);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}