using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementService.Domain.Interfaces;
using WarehouseManagementService.Infra.Data.Data.Seeders;

namespace WarehouseManagementService.Infra.Data.Data
{
    public class AppDbSeeder(AppDbContext context, IUnitOfWork unit)
    {
        public async Task SeedAsync()
        {
            await DefaultSetupSeeder.SeedAsync(context, unit);
            await UsersSeeder.SeedAsync(context, unit);
        }
    }
}
