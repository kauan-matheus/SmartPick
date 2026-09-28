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

        public async Task<ServiceResponse<List<OrderModel>>> Consultar()
        {
            try
            {
                var query = _repository.Consultar<OrderModel>();

                var orders = await query.ToListAsync();

                return ServiceResponse<List<OrderModel>>.Ok(orders);
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<OrderModel>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<OrderModel>> ConsultarPorId(Guid id)
        {
            try
            {
                var result =  await _repository.ConsultarPorId<OrderModel>(id);

                if (result == null)
                {
                    return ServiceResponse<OrderModel>.BadRequest("Pedido nao existe");
                }

                return ServiceResponse<OrderModel>.Ok(result);
            }
            catch (Exception ex)
            {
                return ServiceResponse<OrderModel>.Error(ex.Message);
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
        public async Task<ServiceResponse<OrderModel>> Cadastrar(RequestOrderDto order)
        {
            await _unit.BeginTransaction();

            try
            {
                var novo = new OrderModel
                {
                    Description = order.Description
                };

                await _repository.Cadastrar(novo);
                
                await _unit.Commit();
                await _unit.CommitTransaction();

                return ServiceResponse<OrderModel>.Ok(novo);
            }
            catch (Exception ex)
            {
                await _unit.RollbackTransaction();
                return ServiceResponse<OrderModel>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<OrderModel>> Deletar(Guid id)
        {

            try
            {
                var existente = await _repository.ConsultarPorId<OrderModel>(id);

                if (existente == null)
                {
                    return ServiceResponse<OrderModel>.BadRequest("Usuario nao existe");
                }

                _repository.Excluir(existente);
                var saved = await _unit.Commit();

                if (saved)
                {
                    return ServiceResponse<OrderModel>.Ok(existente);
                }

                return ServiceResponse<OrderModel>.Error("Nao foi possivel deletar esse pedido");
            }
            catch (Exception ex)
            {
                return ServiceResponse<OrderModel>.Error(ex.Message);
            }
        }
    }
}