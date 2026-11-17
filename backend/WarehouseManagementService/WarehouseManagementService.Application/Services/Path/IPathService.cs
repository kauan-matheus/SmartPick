using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Responses;

namespace WarehouseManagementService.Application.Services.Path
{
    public interface IPathService
    {
        Task<ServiceResponse<ResponsePathDto>> ConsultarPedido(Guid id);
    }
}