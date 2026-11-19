using Microsoft.AspNetCore.Mvc;
using Shared.Communication.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Domain.Models;

namespace WarehouseManagementService.Domain.Entities
{
    public class MapModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public MapClass Data { get; set; } = null!;

        public RequestPathDto ToRequestPathDto()
        {
            return new RequestPathDto
            {
                Map = Data.Map,
                Start = Data.Start,
                End = Data.End,
                Pickups = Data.Pickups.Select(p => p.ToDto()).ToList()
            };
        }

    }
}