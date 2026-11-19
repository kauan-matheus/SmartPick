using Shared.Communication.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseManagementService.Domain.Models
{
    public class PickupClass
    {
        public string Nome { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Ponto Posicao { get; set; } = null!;
        
        public PickupDto ToDto()
        {
            return new PickupDto
            {
                Nome = Nome,
                Posicao = Posicao
            };
        }
    }
}