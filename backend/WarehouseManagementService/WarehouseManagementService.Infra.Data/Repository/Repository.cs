using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Domain.Interfaces;
using WarehouseManagementService.Infra.Data.Data;

namespace WarehouseManagementService.Infra.Data.Repository
{
    public class Repository : IRepository
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public virtual IQueryable<T> Consultar<T>() where T : class
        {
            return _context.Set<T>().AsQueryable(); 
        }

        public async Task<T?> ConsultarPorId<T>(Guid id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<bool> Cadastrar<T>(T model) where T : class
        {
            await _context.Set<T>().AddAsync(model);
            return true;
        }

        public bool Editar<T>(T model) where T : class
        {
            _context.Set<T>().Update(model);
            return true;
        }

        public bool Excluir<T>(T model) where T : class
        {
            _context.Set<T>().Remove(model);
            return true;
        }
    }
}