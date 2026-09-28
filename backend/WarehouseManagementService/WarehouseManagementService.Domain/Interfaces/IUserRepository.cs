using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Domain.Entities;

namespace WarehouseManagementService.Domain.Interfaces
{
    public interface IUserRepository : IRepository
    {
        Task<UserModel?> ConsultarUsuarioPorEmail(string email);
        Task<bool> ExisteUsuarioPorEmail(string email);
    }
}