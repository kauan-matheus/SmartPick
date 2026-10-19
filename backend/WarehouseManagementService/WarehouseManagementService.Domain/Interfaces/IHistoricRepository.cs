using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Domain.Entities;

namespace WarehouseManagementService.Domain.Interfaces
{
    public interface IHistoricRepository : IRepository
    {
        IQueryable<HistoricOrderModel> ConsultarOrders();
        IQueryable<HistoricTaskModel> ConsultarTasks();
    }
}