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
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<UserModel?> ConsultarUsuarioPorEmail(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == email);
        }

        public async Task<bool> ExisteUsuarioPorEmail(string email)
        {
            return await _context.Usuarios.AnyAsync(usuario => usuario.Email == email);
        }
    }
}