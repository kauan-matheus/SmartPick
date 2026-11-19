using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Communication.Dto.Request;
using Shared.Communication.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Communication.Enums;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Interfaces;

namespace WarehouseManagementService.Application.Services.Path
{
    public class PathService : IPathService
    {
        private RequestPathDto setup = new RequestPathDto
            {
                Map = new string[][]
                {
                    new[] { ".", ".", ".", "#", ".", ".", ".", ".", ".", "." },
                    new[] { ".", "#", ".", "#", ".", "#", "#", "#", ".", "." },
                    new[] { ".", "#", ".", ".", ".", ".", ".", "#", ".", "." },
                    new[] { ".", "#", "#", "#", "#", "#", ".", "#", ".", "." },
                    new[] { ".", ".", ".", ".", ".", "#", ".", ".", ".", "." },
                    new[] { "#", "#", ".", "#", ".", "#", "#", "#", ".", "#" },
                    new[] { ".", ".", ".", "#", ".", ".", ".", ".", ".", "." },
                    new[] { ".", "#", ".", ".", ".", "#", ".", "#", "#", "." },
                    new[] { ".", "#", "#", "#", ".", ".", ".", ".", ".", "." },
                    new[] { ".", ".", ".", ".", ".", "#", ".", ".", ".", "." }
                },

                Start = new Ponto
                {
                    X = 0,
                    Y = 0
                },

                End = new Ponto
                {
                    X = 9,
                    Y = 9
                },

                Pickups = new List<PickupDto>{}
            };
        
        private List<Pickup> Pickups = new List<Pickup>
            {
                new Pickup
                {
                    Nome = "P1",
                    ProductId = new Guid("047c8ea9-d16b-43c7-f586-9e1a2b4c7d38"),
                    Posicao = new Ponto { X = 5, Y = 2 }
                },
                new Pickup
                {
                    Nome = "P2",
                    ProductId = new Guid("158d9fba-e27c-44d8-a697-0f2b3c5d8e49"),
                    Posicao = new Ponto { X = 8, Y = 4 }
                },
                new Pickup
                {
                    Nome = "P3",
                    ProductId = new Guid("269e0acb-f38d-45e9-b7a8-1a3c4d6e9f50"),
                    Posicao = new Ponto { X = 2, Y = 6 }
                },
                new Pickup
                {
                    Nome = "P4",
                    ProductId = new Guid("2914b54f-1169-4699-8e9a-21e483e7e3f2"),
                    Posicao = new Ponto { X = 1, Y = 0 }
                },
                new Pickup
                {
                    Nome = "P5",
                    ProductId = new Guid("a81c2e43-7b95-4d61-9f20-3e5a6b8c1d72"),
                    Posicao = new Ponto { X = 2, Y = 0 }
                },
                new Pickup
                {
                    Nome = "P6",
                    ProductId = new Guid("b92d3f54-8c16-4e72-a031-4f6b7c9d2e83"),
                    Posicao = new Ponto { X = 4, Y = 0 }
                },
                new Pickup
                {
                    Nome = "P7",
                    ProductId = new Guid("c03e4a65-9d27-4f83-b142-5a7c8d0e3f94"),
                    Posicao = new Ponto { X = 5, Y = 0 }
                },
                new Pickup
                {
                    Nome = "P8",
                    ProductId = new Guid("d14f5b76-ae38-4094-c253-6b8d9e1f4a05"),
                    Posicao = new Ponto { X = 6, Y = 0 }
                },
                new Pickup
                {
                    Nome = "P9",
                    ProductId = new Guid("e25a6c87-bf49-41a5-d364-7c9e0f2a5b16"),
                    Posicao = new Ponto { X = 7, Y = 0 }
                },
                new Pickup
                {
                    Nome = "P10",
                    ProductId = new Guid("f36b7d98-c05a-42b6-e475-8d0f1a3b6c27"),
                    Posicao = new Ponto { X = 8, Y = 0 }
                },
            };

        private readonly IOrderRepository _repository;
        private readonly IUnitOfWork _unit;
        private readonly string _pathFinderUrl;

        public PathService(IConfiguration configuration, IOrderRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unit = unitOfWork;

            _pathFinderUrl = configuration["Grpc:PathFinderUrl"] ?? "http://localhost:5003";
        }

        public async Task<ServiceResponse<ResponsePathDto>> ConsultarPedido(Guid id)
        {
            try
            {
                var order =  await _repository.ConsultarPorId<OrderModel>(id);

                if (order == null)
                {
                    return ServiceResponse<ResponsePathDto>.BadRequest("Pedido invalido");
                }

                var tasks = await _repository.ConsultarTasks(order.Id).ToListAsync();

                if (tasks.Count() <= 0)
                {
                    return ServiceResponse<ResponsePathDto>.BadRequest("Pedido precisa possuir tarefas");
                }

                var lightTasks = new List<TaskModel>();
                var heavyTasks = new List<TaskModel>();

                var lightPickups = new List<Pickup>();
                var heavyPickups = new List<Pickup>();

                foreach (var task in tasks)
                {
                    var product = await _repository.ConsultarPorId<ProductModel>(task.ProductId);
                    if (product == null) {continue;}

                    if (product.Access == ProductAccessEnum.LIGHT){lightTasks.Add(task);}
                    else{heavyTasks.Add(task);}
                }

                var channel = GrpcChannel.ForAddress(_pathFinderUrl);
                var client = new HeldKarpCall.HeldKarpCallClient(channel);

                if (lightTasks.Count() > 0)
                {
                    lightPickups = Pickups.Where(p => lightTasks.Any(t => t.ProductId == p.ProductId)).ToList();

                    var request1 = new RequestPathDto
                    {
                        Map = setup.Map,
                        Start = setup.Start,
                        End = setup.End,
                        Pickups = lightPickups.Select(p => p.ToDto()).ToList()
                    };

                    var input1 = PathFinderLib.ToProto(request1);

                    var reply1 = await client.HeldCarpCallAsync(input1);

                    lightPickups = lightPickups.OrderBy(p => reply1.Path.IndexOf(p.Posicao.X * 10 + p.Posicao.Y)).ToList();

                    lightTasks = lightTasks.OrderBy(t => lightPickups.IndexOf(lightPickups.First(p => p.ProductId == t.ProductId))).ToList();
                }
                if (heavyTasks.Count() > 0)
                {
                    heavyPickups = Pickups.Where(p => heavyTasks.Any(t => t.ProductId == p.ProductId)).ToList();

                    var request2 = new RequestPathDto
                    {
                        Map = setup.Map,
                        Start = setup.Start,
                        End = setup.End,
                        Pickups = heavyPickups.Select(p => p.ToDto()).ToList()
                    };

                    var input2 = PathFinderLib.ToProto(request2);

                    var reply2 = await client.HeldCarpCallAsync(input2);

                    heavyPickups = heavyPickups.OrderBy(p => reply2.Path.IndexOf(p.Posicao.X * 10 + p.Posicao.Y)).ToList();

                    heavyTasks = heavyTasks.OrderBy(t => heavyPickups.IndexOf(heavyPickups.First(p => p.ProductId == t.ProductId))).ToList();
                }
                
                var result = new ResponsePathDto
                {
                    LightTasks = lightTasks.Select(t => t.ToPathDto(lightPickups.First(p => p.ProductId == t.ProductId).Posicao)).ToList(),
                    HeavyTasks = heavyTasks.Select(t => t.ToPathDto(heavyPickups.First(p => p.ProductId == t.ProductId).Posicao)).ToList()
                };

                return ServiceResponse<ResponsePathDto>.Ok(result);
            }
            catch (Exception ex)
            {
                return ServiceResponse<ResponsePathDto>.Error(ex.Message);
            }
        }
        
        private class Pickup
        {
            public required string Nome { get; set; }
            public required Guid ProductId { get; set; }
            public required Ponto Posicao { get; set; }

            public PickupDto ToDto()
            {
                return new PickupDto
                {
                    Nome = Nome,
                    Posicao = Posicao
                };
            }
        }
    }

    
}