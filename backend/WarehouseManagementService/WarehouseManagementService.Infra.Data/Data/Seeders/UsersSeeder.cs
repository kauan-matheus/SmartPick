using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementService.Communication.Enums;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Interfaces;

namespace WarehouseManagementService.Infra.Data.Data.Seeders
{
    public class UsersSeeder
    {
        public static async Task SeedAsync(AppDbContext context, IUnitOfWork unit)
        {
            if (await context.Usuarios.AnyAsync())
            {
                return;
            }

            await unit.BeginTransaction();

            try
            {
                var usuarios = new List<UserModel>
                {
                    new UserModel
                    {
                        Name = "Carlos Eduardo Silva",
                        Email = "carlos.silva@gmail.com",
                        PasswordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword("123456")),
                        Type = UserTypeEnum.EMPLOYEE
                    },
                    new UserModel
                    {
                        Name = "Mariana Souza Lima",
                        Email = "mariana.lima@gmail.com",
                        PasswordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword("123456")),
                        Type = UserTypeEnum.EMPLOYEE
                    },
                    new UserModel
                    {
                        Name = "Rafael Oliveira Santos",
                        Email = "rafael.santos@gmail.com",
                        PasswordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword("123456")),
                        Type = UserTypeEnum.EMPLOYEE
                    },
                    new UserModel
                    {
                        Name = "Juliana Costa Pereira",
                        Email = "juliana.pereira@gmail.com",
                        PasswordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword("123456")),
                        Type = UserTypeEnum.EMPLOYEE
                    },
                    new UserModel
                    {
                        Name = "Thiago Almeida Rocha",
                        Email = "thiago.rocha@gmail.com",
                        PasswordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword("123456")),
                        Type = UserTypeEnum.EMPLOYEE
                    },
                    new UserModel
                    {
                        Name = "gestor",
                        Email = "gestor@gmail.com",
                        PasswordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword("123456")),
                        Type = UserTypeEnum.MANAGER
                    }
                };

                await unit.Commit();
                await unit.CommitTransaction();
            }
            catch
            {
                await unit.RollbackTransaction();
            }
        }
        
    }
}