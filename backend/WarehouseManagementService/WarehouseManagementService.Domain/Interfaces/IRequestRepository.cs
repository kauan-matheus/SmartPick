using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Domain.Entities;

namespace WarehouseManagementService.Domain.Interfaces
{
    public interface IRequestRepository : IRepository
    {
        IQueryable<RequestModel> Consultar();
        Task<OrderModel?> ConsultarOrder(Guid id);
    }
}