using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseManagementService.Communication.Dto.Responses
{
    public class ResponseProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}