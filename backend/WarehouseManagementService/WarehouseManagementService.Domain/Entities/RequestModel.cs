using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Domain.Entities
{
    public class RequestModel
    {
        public Guid Id { get; set; }
        public string Justification { get; set; } = string.Empty;
        public RequestStatusEnum Status { get; set; }
        public RequestObjectiveEnum Objective { get; set; }
        public Guid OrderId { get; set; }
        public OrderModel Order { get; set; } = null!;

        public ResponseRequestDto ToDto()
        {
            return new ResponseRequestDto
            {
                Id = Id,
                Justification = Justification,
                Status = Status,
                Objective = Objective,
                Order = Order.ToDto()
            };
        }
    }
}