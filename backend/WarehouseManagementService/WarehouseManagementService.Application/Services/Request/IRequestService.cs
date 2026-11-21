using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Requests;
using WarehouseManagementService.Communication.Dto.Responses;

namespace WarehouseManagementService.Application.Services.Request
{
    public interface IRequestService
    {
        Task<ServiceResponse<List<ResponseRequestDto>>> Consultar();
        Task<ServiceResponse<ResponseRequestDto>> ConsultarPorId(Guid id);
        Task<ServiceResponse<ResponseRequestDto>> Cadastrar(RequestRequestDto request);
        Task<ServiceResponse<bool>> ConfirmRequest(Guid id, Guid userId);
        Task<ServiceResponse<bool>> RejectRequest(Guid id);
    }
}