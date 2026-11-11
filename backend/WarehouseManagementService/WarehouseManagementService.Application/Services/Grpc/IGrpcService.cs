using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Communication.Dto.Request;
using WarehouseManagementService.Communication.Dto.Responses;

namespace WarehouseManagementService.Application.Services.Grpc
{
    public interface IGrpcService
    {
        Task<ServiceResponse<string>> TesteGrpc();
        Task<ServiceResponse<List<int>>> HeldKarp(RequestPathDto request);
    }
}