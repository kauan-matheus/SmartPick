using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Communication.Dto.Request;
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
        
        private List<PickupDto> Pickups = new List<PickupDto>
            {
                new PickupDto
                {
                    Nome = "Produto A",
                    Posicao = new Ponto { X = 4, Y = 2 }
                },
                new PickupDto
                {
                    Nome = "Produto B",
                    Posicao = new Ponto { X = 8, Y = 4 }
                },
                new PickupDto
                {
                    Nome = "Produto C",
                    Posicao = new Ponto { X = 2, Y = 6 }
                },
                new PickupDto
                {
                    Nome = "Produto D",
                    Posicao = new Ponto { X = 6, Y = 8 }
                }
            };

        private readonly IRepository _repository;
        private readonly IUnitOfWork _unit;

        public PathService(IRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unit = unitOfWork;
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

                var lightTasks = new List<TaskModel>();
                var heavyTasks = new List<TaskModel>();

                foreach (var task in order.Tasks)
                {
                    var product = await _repository.ConsultarPorId<ProductModel>(task.ProductId);
                    if (product == null) {continue;}

                    if (product.Access == ProductAccessEnum.LIGHT){lightTasks.Add(task);}
                    else{heavyTasks.Add(task);}
                }

                return ServiceResponse<ResponsePathDto>.Ok(new ResponsePathDto());
            }
            catch (Exception ex)
            {
                return ServiceResponse<ResponsePathDto>.Error(ex.Message);
            }
        }
    }
}