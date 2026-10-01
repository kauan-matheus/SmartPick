using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Requests;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Domain.Entities;

namespace WarehouseManagementService.Application.Services.Product
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ResponseProductDto>>> Consultar();
        Task<ServiceResponse<ResponseProductDto>> ConsultarPorId(Guid id);
        Task<ServiceResponse<ResponseProductDto>> Cadastrar(RequestProductDto productDto);
        Task<ServiceResponse<ResponseProductDto>> Deletar(Guid id);
    }
}