using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Domain.Interfaces;

namespace WarehouseManagementService.Application.Services.Historic
{
    public class HistoricService : IHistoricService
    {
        private readonly IHistoricRepository _repository;
        private readonly IUnitOfWork _unit;

        public HistoricService(IHistoricRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unit = unitOfWork;
        }

        public async Task<ServiceResponse<List<ResponseHistoricOrderDto>>> ConsultarHistoricoPedidos()
        {
            try
            {
                var result = await _repository.ConsultarOrders().ToListAsync();

                return ServiceResponse<List<ResponseHistoricOrderDto>>.Ok([.. result.Select(t => t.Dtolize())]);
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<ResponseHistoricOrderDto>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<List<ResponseHistoricTaskDto>>> ConsultarHistoricoTarefas()
        {
            try
            {
                var result = await _repository.ConsultarTasks().ToListAsync();

                return ServiceResponse<List<ResponseHistoricTaskDto>>.Ok([.. result.Select(t => t.Dtolize())]);
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<ResponseHistoricTaskDto>>.Error(ex.Message);
            }
        }
    }
}