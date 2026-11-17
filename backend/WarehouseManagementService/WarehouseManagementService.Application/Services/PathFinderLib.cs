using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Communication.Dto.Request;
using Shared.Communication.Protos;

namespace WarehouseManagementService.Application.Services
{
    public class PathFinderLib
    {
        public static CallRequest ToProto(RequestPathDto dto)
        {
            var request = new CallRequest
            {
                Map = ToProtoMap(dto.Map),
                Start = new Position { X = dto.Start.X, Y = dto.Start.Y },
                End = new Position { X = dto.End.X, Y = dto.End.Y }
            };

            request.Pickups.AddRange(dto.Pickups.Select(ToPickup));

            return request;
        }

        public static Map ToProtoMap(string[][] cells)
        {
            if (cells.Length == 0 || cells[0].Length == 0)
                throw new ArgumentException("O mapa não pode ser vazio.");

            int height = cells.Length;
            int width = cells[0].Length;

            if (cells.Any(row => row.Length != width))
                throw new ArgumentException("Todas as linhas do mapa devem ter o mesmo tamanho.");

            var map = new Map { Width = width, Height = height };

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map.Cells.Add(cells[y][x] == "#" ? 1 : 0);

            return map;
        }

        public static Pickup ToPickup(PickupDto pickup) => new()
        {
            Name = pickup.Nome,
            Position = new Position { X = pickup.Posicao.X, Y = pickup.Posicao.Y }
        };
    }
}