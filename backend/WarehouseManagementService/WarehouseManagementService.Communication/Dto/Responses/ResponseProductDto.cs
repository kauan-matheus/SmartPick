using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Communication.Dto.Responses
{
    public class ResponseProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ProductAccessEnum Access { get; set; }
    }
}