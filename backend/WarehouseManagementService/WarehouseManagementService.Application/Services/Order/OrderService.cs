using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementService.Communication.Dto.Requests;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Interfaces;

namespace WarehouseManagementService.Application.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IUnitOfWork _unit;

        public OrderService(IOrderRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unit = unitOfWork;
        }

        public async Task<ServiceResponse<List<ResponseOrderDto>>> Consultar()
        {
            try
            {
                var query = _repository.Consultar<OrderModel>();

                var orders = await query.ToListAsync();

                return ServiceResponse<List<ResponseOrderDto>>.Ok([.. orders.Select(o => o.Dtolize())]);
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<ResponseOrderDto>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<ResponseOrderDto>> ConsultarPorId(Guid id)
        {
            try
            {
                var result =  await _repository.ConsultarPorId<ResponseOrderDto>(id);

                if (result == null)
                {
                    return ServiceResponse<ResponseOrderDto>.BadRequest("Pedido nao existe");
                }

                return ServiceResponse<ResponseOrderDto>.Ok(result);
            }
            catch (Exception ex)
            {
                return ServiceResponse<ResponseOrderDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<List<ResponseTaskDto>>> ConsultarTasks(Guid id)
        {
            try
            {
                var result = await _repository.ConsultarTasks(id).ToListAsync();

                if (result == null)
                {
                    return ServiceResponse<List<ResponseTaskDto>>.BadRequest("Pedido nao existe");
                }

                return ServiceResponse<List<ResponseTaskDto>>.Ok([.. result.Select(t => t.Dtolize())]);
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<ResponseTaskDto>>.Error(ex.Message);
            }
        }
        public async Task<ServiceResponse<ResponseOrderDto>> Cadastrar(RequestOrderDto order)
        {
            await _unit.BeginTransaction();

            try
            {
                var novo = new OrderModel
                {
                    Description = order.Description
                };

                await _repository.Cadastrar(novo);

                foreach (var task in order.Tasks)
                {
                    var novoTask = new TaskModel
                    {
                        Description = task.Description,
                        Quantity = task.Quantity,
                        ProductId = task.ProductId,
                        OrderId = novo.Id
                    };

                    await _repository.Cadastrar(novoTask);
                }
                
                await _unit.Commit();
                await _unit.CommitTransaction();

                return ServiceResponse<ResponseOrderDto>.Ok(novo.Dtolize());
            }
            catch (Exception ex)
            {
                await _unit.RollbackTransaction();
                return ServiceResponse<ResponseOrderDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<ResponseOrderDto>> Deletar(Guid id)
        {

            try
            {
                var existente = await _repository.ConsultarPorId<OrderModel>(id);

                if (existente == null)
                {
                    return ServiceResponse<ResponseOrderDto>.BadRequest("Usuario nao existe");
                }

                _repository.Excluir(existente);
                var saved = await _unit.Commit();

                if (saved)
                {
                    return ServiceResponse<ResponseOrderDto>.Ok(existente.Dtolize());
                }

                return ServiceResponse<ResponseOrderDto>.Error("Nao foi possivel deletar esse pedido");
            }
            catch (Exception ex)
            {
                return ServiceResponse<ResponseOrderDto>.Error(ex.Message);
            }
        }
    }
}