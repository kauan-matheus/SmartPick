using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Interfaces;
using WarehouseManagementService.Infra.Data.Data;

namespace WarehouseManagementService.Infra.Data.Repository
{
    public class OrderRepository : Repository, IOrderRepository
    {
        public OrderRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public IQueryable<OrderModel> Consultar()
        {
            return _context.Pedidos.AsQueryable().Include(p => p.Tasks); 
        }

        public IQueryable<TaskModel> ConsultarTasks(Guid id)
        {
            return _context.Tarefas
                .Where(t => t.OrderId == id)
                .OrderByDescending(t => t.Id)
                .AsQueryable();
        }
    }
}