using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Communication.Dto.Request;
using WarehouseManagementService.Communication.Enums;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Interfaces;
using WarehouseManagementService.Domain.Models;

namespace WarehouseManagementService.Infra.Data.Data.Seeders
{
    internal class DefaultSetupSeeder
    {
        public static async Task SeedAsync(AppDbContext context, IUnitOfWork unit)
        {
            if (await context.Produtos.AnyAsync() && await context.Mapas.AnyAsync())
            {
                return;
            }
            await unit.BeginTransaction();

            try
            {
                var produtos = new List<ProductModel>
                {
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Iphone 17 Pro Max",
                        Access = ProductAccessEnum.LIGHT
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Samsung Galaxy S25 Ultra",
                        Access = ProductAccessEnum.LIGHT
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "MacBook Air M4",
                        Access = ProductAccessEnum.LIGHT
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "iPad Pro M4",
                        Access = ProductAccessEnum.LIGHT
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "PlayStation 5 Slim",
                        Access = ProductAccessEnum.LIGHT
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Xbox Series X",
                        Access = ProductAccessEnum.LIGHT
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Apple Watch Series 10",
                        Access = ProductAccessEnum.LIGHT
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Sony WH-1000XM6",
                        Access = ProductAccessEnum.LIGHT
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Dell XPS 15",
                        Access = ProductAccessEnum.LIGHT
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Nintendo Switch OLED",
                        Access = ProductAccessEnum.LIGHT
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Geladeira Samsung Side by Side",
                        Access = ProductAccessEnum.HEAVY
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Maquina de Lavar LG 15kg",
                        Access = ProductAccessEnum.HEAVY
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Fogao Electrolux 5 Bocas",
                        Access = ProductAccessEnum.HEAVY
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "TV LG OLED 83 polegadas",
                        Access = ProductAccessEnum.HEAVY
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Ar-Condicionado Split 18000 BTU",
                        Access = ProductAccessEnum.HEAVY
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Sofa Retratil 3 Lugares",
                        Access = ProductAccessEnum.HEAVY
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Guarda-Roupa 6 Portas",
                        Access = ProductAccessEnum.HEAVY
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Home Theater Sony 1000W",
                        Access = ProductAccessEnum.HEAVY
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Freezer Horizontal 400L",
                        Access = ProductAccessEnum.HEAVY
                    },
                    new ProductModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Lava e Seca Samsung 21kg",
                        Access = ProductAccessEnum.HEAVY
                    }
                };

                await context.AddRangeAsync(produtos);

                var pickups = new List<PickupClass>
                {
                    new PickupClass
                    {
                        Nome = "P1",
                        ProductId = produtos[0].Id,
                        Posicao = new Ponto { X = 0, Y = 0 }
                    },
                    new PickupClass
                    {
                        Nome = "P2",
                        ProductId = produtos[1].Id,
                        Posicao = new Ponto { X = 3, Y = 1 }
                    },
                    new PickupClass
                    {
                        Nome = "P3",
                        ProductId = produtos[2].Id,
                        Posicao = new Ponto { X = 8, Y = 2 }
                    },
                    new PickupClass
                    {
                        Nome = "P4",
                        ProductId = produtos[3].Id,
                        Posicao = new Ponto { X = 13, Y = 1 }
                    },
                    new PickupClass
                    {
                        Nome = "P5",
                        ProductId = produtos[4].Id,
                        Posicao = new Ponto { X = 15, Y = 3 }
                    },
                    new PickupClass
                    {
                        Nome = "P6",
                        ProductId = produtos[5].Id,
                        Posicao = new Ponto { X = 2, Y = 4 }
                    },
                    new PickupClass
                    {
                        Nome = "P7",
                        ProductId = produtos[6].Id,
                        Posicao = new Ponto { X = 5, Y = 5 }
                    },
                    new PickupClass
                    {
                        Nome = "P8",
                        ProductId = produtos[7].Id,
                        Posicao = new Ponto { X = 8, Y = 4 }
                    },
                    new PickupClass
                    {
                        Nome = "P9",
                        ProductId = produtos[8].Id,
                        Posicao = new Ponto { X = 10, Y = 5 }
                    },
                    new PickupClass
                    {
                        Nome = "P10",
                        ProductId = produtos[9].Id,
                        Posicao = new Ponto { X = 13, Y = 4 }
                    },
                    new PickupClass
                    {
                        Nome = "P11",
                        ProductId = produtos[10].Id,
                        Posicao = new Ponto { X = 2, Y = 7 }
                    },
                    new PickupClass
                    {
                        Nome = "P12",
                        ProductId = produtos[11].Id,
                        Posicao = new Ponto { X = 7, Y = 7 }
                    },
                    new PickupClass
                    {
                        Nome = "P13",
                        ProductId = produtos[12].Id,
                        Posicao = new Ponto { X = 12, Y = 7 }
                    },
                    new PickupClass
                    {
                        Nome = "P14",
                        ProductId = produtos[13].Id,
                        Posicao = new Ponto { X = 0, Y = 9 }
                    },
                    new PickupClass
                    {
                        Nome = "P15",
                        ProductId = produtos[14].Id,
                        Posicao = new Ponto { X = 5, Y = 10 }
                    },
                    new PickupClass
                    {
                        Nome = "P16",
                        ProductId = produtos[15].Id,
                        Posicao = new Ponto { X = 8, Y = 9 }
                    },
                    new PickupClass
                    {
                        Nome = "P17",
                        ProductId = produtos[16].Id,
                        Posicao = new Ponto { X = 13, Y = 10 }
                    },
                    new PickupClass
                    {
                        Nome = "P18",
                        ProductId = produtos[17].Id,
                        Posicao = new Ponto { X = 4, Y = 12 }
                    },
                    new PickupClass
                    {
                        Nome = "P19",
                        ProductId = produtos[18].Id,
                        Posicao = new Ponto { X = 10, Y = 12 }
                    },
                    new PickupClass
                    {
                        Nome = "P20",
                        ProductId = produtos[19].Id,
                        Posicao = new Ponto { X = 6, Y = 14 }
                    }
                };

                var mapData = new MapClass
                {
                    Map = new string[][]
                    {
                        new[] { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
                        new[] { ".", "#", "#", ".", "#", "#", "#", "#", ".", "#", "#", "#", "#", ".", "#", "." },
                        new[] { ".", "#", "#", ".", "#", "#", "#", "#", ".", "#", "#", "#", "#", ".", "#", "." },
                        new[] { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
                        new[] { ".", "#", ".", "#", "#", ".", "#", ".", ".", "#", ".", "#", "#", ".", "#", "." },
                        new[] { ".", "#", ".", "#", "#", ".", "#", ".", ".", "#", ".", "#", "#", ".", "#", "." },
                        new[] { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
                        new[] { "#", "#", ".", "#", "#", "#", "#", ".", "#", "#", "#", "#", ".", "#", "#", "#" },
                        new[] { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
                        new[] { ".", "#", "#", "#", "#", ".", "#", "#", ".", "#", "#", "#", "#", ".", "#", "." },
                        new[] { ".", "#", "#", "#", "#", ".", "#", "#", ".", "#", "#", "#", "#", ".", "#", "." },
                        new[] { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
                        new[] { ".", "#", ".", "#", ".", "#", ".", "#", ".", "#", ".", "#", ".", "#", ".", "#" },
                        new[] { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
                        new[] { ".", "#", "#", ".", "#", "#", ".", "#", "#", ".", "#", "#", ".", "#", "#", "." },
                        new[] { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." }
                    },
                    Start = new Ponto { X = 0, Y = 0 },
                    End = new Ponto { X = 15, Y = 15 },
                    Pickups = pickups
                };

                await context.AddAsync(new MapModel
                {
                    Name = "DefaultMap",
                    Data = mapData
                });

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