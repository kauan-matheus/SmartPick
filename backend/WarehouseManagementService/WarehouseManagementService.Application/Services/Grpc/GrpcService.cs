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
                var input = ToProto(request);

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

        public CallRequest ToProto(RequestPathDto dto)
        {
            var request = new CallRequest
            {
                Map = ToProtoMap(dto.Map),
                Start = new Position { X = dto.Start.X, Y = dto.Start.Y },
                End = new Position { X = dto.End.X, Y = dto.End.Y }
            };

            request.Pickups.AddRange(dto.Pickups.Select(ToPickup));

            return request;
        }

        private Map ToProtoMap(string[][] cells)
        {
            if (cells.Length == 0 || cells[0].Length == 0)
                throw new ArgumentException("O mapa não pode ser vazio.");

            int height = cells.Length;
            int width = cells[0].Length;

            if (cells.Any(row => row.Length != width))
                throw new ArgumentException("Todas as linhas do mapa devem ter o mesmo tamanho.");

            var map = new Map { Width = width, Height = height };

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map.Cells.Add(cells[y][x] == "#" ? 1 : 0);

            return map;
        }

        private static Pickup ToPickup(PickupDto pickup) => new()
        {
            Name = pickup.Nome,
            Position = new Position { X = pickup.Posicao.X, Y = pickup.Posicao.Y }
        };
    }
}