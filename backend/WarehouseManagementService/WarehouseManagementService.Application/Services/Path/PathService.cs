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
using WarehouseManagementService.Domain.Models;

namespace WarehouseManagementService.Application.Services.Path
{
    public class PathService : IPathService
    {
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
                var maps = await _repository.Consultar<MapModel>().ToArrayAsync();

                if (maps.Length == 0)
                {
                    return ServiceResponse<ResponsePathDto>.BadRequest("Nenhum mapa encontrado");
                }

                var defaultMap = maps.FirstOrDefault(m => m.Name == "DefaultMap");

                if (defaultMap == null)
                {
                    return ServiceResponse<ResponsePathDto>.BadRequest("Mapa padrao não encontrado no banco de dados");
                }

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

                var lightPickups = new List<PickupClass>();
                var heavyPickups = new List<PickupClass>();

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
                    lightPickups = defaultMap.Data.Pickups.Where(p => lightTasks.Any(t => t.ProductId == p.ProductId)).ToList();

                    var request1 = defaultMap.ToRequestPathDto();
                    request1.Pickups = lightPickups.Select(p => p.ToDto()).ToList();

                    var input1 = PathFinderLib.ToProto(request1);

                    var reply1 = await client.HeldCarpCallAsync(input1);

                    lightPickups = reply1.Path
                        .Skip(1)
                        .Take(reply1.Path.Count - 2)
                        .Select(index => lightPickups[index - 1])
                        .ToList();

                    lightTasks = lightTasks.OrderBy(t => lightPickups.IndexOf(lightPickups.First(p => p.ProductId == t.ProductId))).ToList();
                }
                if (heavyTasks.Count() > 0)
                {
                    heavyPickups = defaultMap.Data.Pickups.Where(p => heavyTasks.Any(t => t.ProductId == p.ProductId)).ToList();

                    var request2 = defaultMap.ToRequestPathDto();
                    request2.Pickups = heavyPickups.Select(p => p.ToDto()).ToList();

                    var input2 = PathFinderLib.ToProto(request2);

                    var reply2 = await client.HeldCarpCallAsync(input2);

                    heavyPickups = reply2.Path
                        .Skip(1)
                        .Take(reply2.Path.Count - 2)
                        .Select(index => heavyPickups[index - 1])
                        .ToList();

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
    }

    
}