using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Responses;

namespace WarehouseManagementService.Application.Services.Historic
{
    public interface IHistoricService
    {
        Task<ServiceResponse<List<ResponseHistoricOrderDto>>> ConsultarHistoricoPedidos();
        Task<ServiceResponse<List<ResponseHistoricTaskDto>>> ConsultarHistoricoTarefas();
    }
}