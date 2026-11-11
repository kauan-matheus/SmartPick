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

                var matriz = ToMatriz(request.Map);

                int height = matriz.GetLength(0); // linhas
                int width  = matriz.GetLength(1); // colunas
                var map = new Map
                {
                    Width = width,
                    Height = height
                };

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        map.Cells.Add(matriz[y, x] == "#" ? 1 : 0);
                    }
                }

                var input = new CallRequest
                {
                    Map = map,
                    Start = new Position { X = request.Start.X, Y = request.Start.Y },
                    End = new Position { X = request.End.Y, Y = request.End.Y }
                };

                foreach (var pickup in request.Pickups)
                {
                    input.Pickups.Add(new Pickup
                    {
                        Name = pickup.Nome,
                        Position = new Position { X = pickup.Posicao.X, Y = pickup.Posicao.Y }
                    });
                }

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

        public static string[,] ToMatriz(string[][] jagged)
        {
            int h = jagged.Length;
            int w = jagged[0].Length;
            var m = new string[h, w];

            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    m[y, x] = jagged[y][x];

            return m;
        }
    }
}