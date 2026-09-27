using Grpc.Net.Client;
using PathFinderService.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Domain.Entities;

namespace WarehouseManagementService.Application.Services.Grpc
{
    public class GrpcService : IGrpcService
    {
        public async Task<ServiceResponse<string>> TesteGrpc()
        {
            try
            {
                var input = new ConnectionRequest { };

                var channel = GrpcChannel.ForAddress("http://localhost:5003");
                var client = new ConnectionTest.ConnectionTestClient(channel);

                var reply = await client.TestConnectionAsync(input);

                return ServiceResponse<string>.Ok(reply.Message);
            }
            catch
            {
                return ServiceResponse<string>.Error("Connection nao acessivel");
            }
            
        }
    }
}