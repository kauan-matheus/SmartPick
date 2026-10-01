using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Communication.Dto.Responses
{
    public class ResponseOrderDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<ResponseTaskDto> Tasks { get; set; } = new List<ResponseTaskDto>();
        public StatusEnum Status { get; set; }
    }
}