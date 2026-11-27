using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WarehouseManagementService.Communication.Dto.Requests;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Communication.Enums;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Interfaces;

namespace WarehouseManagementService.Application.Services.Request
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _repository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unit;

        public RequestService(IRequestRepository repository, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _orderRepository = orderRepository;
            _unit = unitOfWork;
        }

        public async Task<ServiceResponse<List<ResponseRequestDto>>> Consultar()
        {
            try
            {
                var query = _repository.Consultar();

                var requests = await query.ToListAsync();

                return ServiceResponse<List<ResponseRequestDto>>.Ok([.. requests.Select(p => p.ToDto())]);
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<ResponseRequestDto>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<ResponseRequestDto>> ConsultarPorId(Guid id)
        {
            try
            {
                var result =  await _repository.ConsultarPorId<RequestModel>(id);
                if (result == null){ return ServiceResponse<ResponseRequestDto>.BadRequest("Solicitação nao existe");}
                var order = await _repository.ConsultarPorId<OrderModel>(result.OrderId);
                if (order == null){ return ServiceResponse<ResponseRequestDto>.BadRequest("Solicitação precisa possuir um pedido");}

                return ServiceResponse<ResponseRequestDto>.Ok(result.ToDto());
            }
            catch (Exception ex)
            {
                return ServiceResponse<ResponseRequestDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<ResponseRequestDto>> Cadastrar(RequestRequestDto request)
        {
            var order = await _repository.ConsultarOrder(request.OrderId);

            if (order == null)
            {
                return ServiceResponse<ResponseRequestDto>.BadRequest("Pedido não existe");
            }
            if (order.Status != StatusEnum.WAITING)
            {
                return ServiceResponse<ResponseRequestDto>.BadRequest("Pedido ja nao pode ser cancelado");
            }
            await _unit.BeginTransaction();

            try
            {
                var novo = new RequestModel
                {
                    Justification = request.Justification,
                    Objective = request.Objective,
                    OrderId = request.OrderId
                };

                await _repository.Cadastrar(novo);
                
                await _unit.Commit();
                await _unit.CommitTransaction();

                return ServiceResponse<ResponseRequestDto>.Ok(novo.ToDto());
            }
            catch (Exception ex)
            {
                await _unit.RollbackTransaction();
                return ServiceResponse<ResponseRequestDto>.Error(ex.Message);
            }
            
        }

        public async Task<ServiceResponse<bool>> ConfirmRequest(Guid id, Guid userId)
        {
            var request = await _repository.ConsultarPorId<RequestModel>(id);
            if (request == null)
            {
                return ServiceResponse<bool>.BadRequest("Solicitação não existe");
            }

            var order = await _repository.ConsultarPorId<OrderModel>(request.OrderId);
            if (order == null)
            {
                return ServiceResponse<bool>.BadRequest("Solicitação não possui pedido");
            }

            var tasks = _orderRepository.ConsultarTasks(order.Id);

            await _unit.BeginTransaction();

            try
            {
                switch (request.Objective)
                {
                    case RequestObjectiveEnum.CANCELORDER:
                        order.Status = StatusEnum.CANCELED;
                        _repository.Editar(order);
                        await _repository.Cadastrar(new HistoricOrderModel(order.Id, EventEnum.ORDERCANCELED, userId));

                        foreach (var task in tasks)
                        {
                            task.Status = StatusEnum.CANCELED;
                            _repository.Editar(task);
                            await _repository.Cadastrar(new HistoricTaskModel(task.Id, EventEnum.TASKCANCELED, userId));
                        }

                        request.Status = RequestStatusEnum.CONFIRMADO;
                        _repository.Editar(request);
                        break;
                }
            
                await _unit.Commit();
                await _unit.CommitTransaction();

                return ServiceResponse<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                await _unit.RollbackTransaction();
                return ServiceResponse<bool>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> RejectRequest(Guid id)
        {
            var request = await _repository.ConsultarPorId<RequestModel>(id);
            if (request == null)
            {
                return ServiceResponse<bool>.BadRequest("Solicitação não existe");
            }

            var order = await _repository.ConsultarPorId<OrderModel>(request.OrderId);

            if (order == null)
            {
                return ServiceResponse<bool>.BadRequest("Solicitação não possui pedido");
            }

            await _unit.BeginTransaction();

            try
            {
                request.Status = RequestStatusEnum.NEGADO;
                _repository.Editar(request);
                
                await _unit.Commit();
                await _unit.CommitTransaction();

                return ServiceResponse<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                await _unit.RollbackTransaction();
                return ServiceResponse<bool>.Error(ex.Message);
            }
        }
    }
}