using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementService.Infra.Data.Data.Seeders;

namespace WarehouseManagementService.Infra.Data.Data
{
    public class AppDbSeeder(AppDbContext context)
    {
        public async Task SeedAsync()
        {
            await ProductSeeder.SeedAsync(context);
        }
    }
}
