using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Communication.Dto.Responses
{
    public class ResponseRequestDto
    {
        public Guid Id { get; set; }
        public string Justification { get; set; } = string.Empty;
        public RequestStatusEnum Status { get; set; }
        public RequestObjectiveEnum Objective { get; set; }
        public ResponseOrderDto Order { get; set; } = null!;
    }
}