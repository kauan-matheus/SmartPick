using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Shared.Communication.Dto.Request;
using Shared.Communication.Protos;
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
        private readonly string _pathFinderUrl;

        public GrpcService(IConfiguration configuration)
        {
            _pathFinderUrl = configuration["Grpc:PathFinderUrl"] ?? "http://localhost:5003";
        }

        public async Task<ServiceResponse<string>> TesteGrpc()
        {
            try
            {
                var input = new ConnectionRequest{ };

                var channel = GrpcChannel.ForAddress(_pathFinderUrl);
                var client = new ConnectionTest.ConnectionTestClient(channel);

                var reply = await client.TestConnectionAsync(input);

                return ServiceResponse<string>.Ok(reply.Message);
            }
            catch
            {
                return ServiceResponse<string>.Error("Falha na conexao");
            }
            
        }

        public async Task<ServiceResponse<List<int>>> HeldKarp(RequestPathDto request)
        {
            try
            {
                var input = PathFinderLib.ToProto(request);

                var channel = GrpcChannel.ForAddress(_pathFinderUrl);
                var client = new HeldKarpCall.HeldKarpCallClient(channel);

                var reply = await client.HeldCarpCallAsync(input);

                return ServiceResponse<List<int>>.Ok(reply.Path.ToList());
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<int>>.Error(ex.Message);
            }
        }
    }
}