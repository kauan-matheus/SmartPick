using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Requests;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Domain.Entities;

namespace WarehouseManagementService.Application.Services.Order
{
    public interface IOrderService
    {
        Task<ServiceResponse<List<ResponseOrderDto>>> Consultar();
        Task<ServiceResponse<ResponseOrderDto>> ConsultarPorId(Guid id);
        Task<ServiceResponse<List<ResponseTaskDto>>> ConsultarTasks(Guid id);
        Task<ServiceResponse<ResponseOrderDto>> Cadastrar(RequestOrderDto orderDto);
        Task<ServiceResponse<ResponseOrderDto>> Deletar(Guid id);
    }
}