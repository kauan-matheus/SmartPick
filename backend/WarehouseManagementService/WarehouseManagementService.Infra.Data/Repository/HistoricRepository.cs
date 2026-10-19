using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Interfaces;
using WarehouseManagementService.Infra.Data.Data;

namespace WarehouseManagementService.Infra.Data.Repository
{
    public class HistoricRepository : Repository, IHistoricRepository
    {
        public HistoricRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
        
        public IQueryable<HistoricOrderModel> ConsultarOrders()
        {
            return _context.HistoricoPedidos
                .OrderByDescending(t => t.Id)
                .AsQueryable();
        }

        public IQueryable<HistoricTaskModel> ConsultarTasks()
        {
            return _context.HistoricoTarefas
                .OrderByDescending(t => t.Id)
                .AsQueryable();
        }
    }
}