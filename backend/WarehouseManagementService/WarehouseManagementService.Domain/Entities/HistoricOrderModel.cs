using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Domain.Entities
{
    public class HistoricOrderModel
    {
        public Guid Id { get; set; }
        public EventEnum Event { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public Guid OrderId { get; set; }
        public OrderModel Order { get; set; } = null!;
        public Guid? UserId { get; set; }
        public UserModel? User { get; set; }

        public HistoricOrderModel()
        {}
        public HistoricOrderModel(Guid orderId, EventEnum eventEnum)
        {
            Event = eventEnum;
            OrderId = orderId;
        }
        public HistoricOrderModel(Guid orderId, EventEnum eventEnum, Guid userId)
        {
            Event = eventEnum;
            OrderId = orderId;
            UserId = userId;
        }

        public ResponseHistoricOrderDto Dtolize()
        {
            return new ResponseHistoricOrderDto
            {
                Id = Id,
                Event = Event,
                CreationDate = CreationDate,
                OrderId = OrderId,
                UserId = UserId
            };
        }
    }
}