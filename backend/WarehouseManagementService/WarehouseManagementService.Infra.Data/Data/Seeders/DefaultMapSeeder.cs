using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Communication.Dto.Request;
using WarehouseManagementService.Communication.Enums;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Models;

namespace WarehouseManagementService.Infra.Data.Data.Seeders
{
    internal class DefaultMapSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {

            if (await context.Produtos.AnyAsync() && await context.Mapas.AnyAsync())
            {
                return;
            }

            var produtos = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = new Guid(),
                    Name = "Iphone 17 Pro Max",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid(),
                    Name = "Samsung Galaxy S25 Ultra",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid(),
                    Name = "MacBook Air M4",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid(),
                    Name = "iPad Pro M4",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid(),
                    Name = "PlayStation 5 Slim",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid(),
                    Name = "Xbox Series X",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid(),
                    Name = "Apple Watch Series 10",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid(),
                    Name = "Sony WH-1000XM6",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid(),
                    Name = "Dell XPS 15",
                    Access = ProductAccessEnum.LIGHT
                },
                new ProductModel
                {
                    Id = new Guid(),
                    Name = "Nintendo Switch OLED",
                    Access = ProductAccessEnum.LIGHT
                }
            };

            await context.AddRangeAsync(produtos);

            var pickups = new List<PickupClass>
            {
                new PickupClass
                {
                    Nome = "P1",
                    ProductId = produtos[0].Id,
                    Posicao = new Ponto { X = 5, Y = 2 }
                },
                new PickupClass
                {
                    Nome = "P2",
                    ProductId = produtos[1].Id,
                    Posicao = new Ponto { X = 8, Y = 4 }
                },
                new PickupClass
                {
                    Nome = "P3",
                    ProductId = produtos[2].Id,
                    Posicao = new Ponto { X = 2, Y = 6 }
                },
                new PickupClass
                {
                    Nome = "P4",
                    ProductId = produtos[3].Id,
                    Posicao = new Ponto { X = 1, Y = 0 }
                },
                new PickupClass
                {
                    Nome = "P5",
                    ProductId = produtos[4].Id,
                    Posicao = new Ponto { X = 2, Y = 0 }
                },
                new PickupClass
                {
                    Nome = "P6",
                    ProductId = produtos[5].Id,
                    Posicao = new Ponto { X = 4, Y = 0 }
                },
                new PickupClass
                {
                    Nome = "P7",
                    ProductId = produtos[6].Id,
                    Posicao = new Ponto { X = 6, Y = 0 }
                },
                new PickupClass
                {
                    Nome = "P8",
                    ProductId = produtos[7].Id,
                    Posicao = new Ponto { X = 6, Y = 0 }
                },
                new PickupClass
                {
                    Nome = "P9",
                    ProductId = produtos[8].Id,
                    Posicao = new Ponto { X = 7, Y = 0 }
                },
                new PickupClass
                {
                    Nome = "P10",
                    ProductId = produtos[9].Id,
                    Posicao = new Ponto { X = 8, Y = 0 }
                },
            };

            var mapData = new MapClass
            {
                Map = new string[][]
                {
                    new[] { ".", ".", ".", "#", ".", ".", ".", ".", ".", "." },
                    new[] { ".", "#", ".", "#", ".", "#", "#", "#", ".", "." },
                    new[] { ".", "#", ".", ".", ".", ".", ".", "#", ".", "." },
                    new[] { ".", "#", "#", "#", "#", "#", ".", "#", ".", "." },
                    new[] { ".", ".", ".", ".", ".", "#", ".", ".", ".", "." },
                    new[] { "#", "#", ".", "#", ".", "#", "#", "#", ".", "#" },
                    new[] { ".", ".", ".", "#", ".", ".", ".", ".", ".", "." },
                    new[] { ".", "#", ".", ".", ".", "#", ".", "#", "#", "." },
                    new[] { ".", "#", "#", "#", ".", ".", ".", ".", ".", "." },
                    new[] { ".", ".", ".", ".", ".", "#", ".", ".", ".", "." }
                },
                Start = new Ponto { X = 0, Y = 0 },
                End = new Ponto { X = 9, Y = 9 },
                Pickups = pickups
            };

            await context.AddAsync(new MapModel
            {
                Name = "DefaultMap",
                Data = mapData
            });

            await context.SaveChangesAsync();
            
        }
    }
}