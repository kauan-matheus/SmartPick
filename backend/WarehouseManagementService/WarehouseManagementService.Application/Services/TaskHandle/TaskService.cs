using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Communication.Enums;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Interfaces;

namespace WarehouseManagementService.Application.Services.TaskHandle
{
    public class TaskService : ITaskService
    {
        private readonly IRepository _repository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unit;
        public TaskService(IRepository repository, IOrderRepository orderRepository, IUnitOfWork unit)
        {
            _repository = repository;
            _orderRepository = orderRepository;
            _unit = unit;
        }
        public async Task<ServiceResponse<bool>> ConcluirTask(Guid id, Guid userId)
        {
            var task = await _repository.ConsultarPorId<TaskModel>(id);
            if (task == null){ return ServiceResponse<bool>.BadRequest("Tarefa não existe");}
            if (task.Status != StatusEnum.WAITING){ return ServiceResponse<bool>.BadRequest("Tarefa nao esta em estado que possa ser concluida");}
            var order = await _repository.ConsultarPorId<OrderModel>(task.OrderId);
            if (order == null){ return ServiceResponse<bool>.BadRequest("Tarefa não possui pedido");}
            var tasks = _orderRepository.ConsultarTasks(order.Id);

            await _unit.BeginTransaction();

            try
            {
                task.Status = StatusEnum.FINISHED;
                _repository.Editar(task);
                await _repository.Cadastrar(new HistoricTaskModel(task.Id, EventEnum.TASKFINISHED, userId));

                if (tasks.Where(t => t.Id != task.Id && t.Status != StatusEnum.FINISHED).Count() <= 0)
                {
                    order.Status = StatusEnum.FINISHED;
                    _repository.Editar(order);
                    await _repository.Cadastrar(new HistoricOrderModel(order.Id, EventEnum.ORDERFINISHED, userId));
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
    }
}