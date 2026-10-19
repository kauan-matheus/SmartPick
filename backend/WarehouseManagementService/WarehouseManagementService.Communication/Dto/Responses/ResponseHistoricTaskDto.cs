using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Communication.Dto.Responses
{
    public class ResponseHistoricTaskDto
    {
        public Guid Id { get; set; }
        public EventEnum Event { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
    }
}