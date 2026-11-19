using Shared.Communication.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementService.Domain.Models
{
    public class MapClass
    {
        public string[][] Map { get; set; } = null!;
        public Ponto Start { get; set; } = null!;
        public Ponto End { get; set; } = null!;
        public ICollection<PickupClass> Pickups { get; set; } = new List<PickupClass>();
    }
}
