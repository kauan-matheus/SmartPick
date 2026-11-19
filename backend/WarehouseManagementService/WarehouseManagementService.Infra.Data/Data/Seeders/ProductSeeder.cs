using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementService.Communication.Enums;
using WarehouseManagementService.Domain.Entities;

namespace WarehouseManagementService.Infra.Data.Data.Seeders
{
    internal class ProductSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Produtos.AnyAsync())
            {
                return;
            }

            var produtos = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = new Guid("2914b54f-1169-4699-8e9a-21e483e7e3f2"),
                    Name = "Iphone 17 Pro Max",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid("a81c2e43-7b95-4d61-9f20-3e5a6b8c1d72"),
                    Name = "Samsung Galaxy S25 Ultra",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid("b92d3f54-8c16-4e72-a031-4f6b7c9d2e83"),
                    Name = "MacBook Air M4",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid("c03e4a65-9d27-4f83-b142-5a7c8d0e3f94"),
                    Name = "iPad Pro M4",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid("d14f5b76-ae38-4094-c253-6b8d9e1f4a05"),
                    Name = "PlayStation 5 Slim",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid("e25a6c87-bf49-41a5-d364-7c9e0f2a5b16"),
                    Name = "Xbox Series X",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid("f36b7d98-c05a-42b6-e475-8d0f1a3b6c27"),
                    Name = "Apple Watch Series 10",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid("047c8ea9-d16b-43c7-f586-9e1a2b4c7d38"),
                    Name = "Sony WH-1000XM6",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid("158d9fba-e27c-44d8-a697-0f2b3c5d8e49"),
                    Name = "Dell XPS 15",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid("269e0acb-f38d-45e9-b7a8-1a3c4d6e9f50"),
                    Name = "Nintendo Switch OLED",
                    Access = ProductAccessEnum.LIGHT
                }
            };

            await context.AddRangeAsync(produtos);
            await context.SaveChangesAsync();
            
        }
    }
}