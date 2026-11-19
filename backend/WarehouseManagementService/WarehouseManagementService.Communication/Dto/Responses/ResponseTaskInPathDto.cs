using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Enums;
using Shared.Communication.Dto.Request;

namespace WarehouseManagementService.Communication.Dto.Responses
{

    public class ResponseTaskInPathDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public StatusEnum Status { get; set; }
        public int Quantity { get; set; }
        public Ponto Position { get; set; }
        public Guid ProductId { get; set; }
    }

}