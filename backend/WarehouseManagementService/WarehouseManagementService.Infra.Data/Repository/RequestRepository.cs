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
    public class RequestRepository : Repository, IRequestRepository
    {
        public RequestRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public IQueryable<RequestModel> Consultar()
        {
            return _context.Solicitacoes.AsQueryable().Include(r => r.Order.Tasks); 
        }

        public async Task<OrderModel?> ConsultarOrder(Guid id)
        {
            return await _context.Pedidos.Include(o => o.Tasks).FirstOrDefaultAsync(o => o.Id == id); 
        }
    }
}