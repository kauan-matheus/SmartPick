using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Responses;

namespace WarehouseManagementService.Application.Services.TaskHandle
{
    public interface ITaskService
    {
        Task<ServiceResponse<bool>> ConcluirTask(Guid id, Guid userId);
    }
}