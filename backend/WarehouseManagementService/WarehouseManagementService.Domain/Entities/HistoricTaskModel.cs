using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Domain.Entities
{
    public class HistoricTaskModel
    {
        public Guid Id { get; set; }
        public EventEnum Event { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public Guid TaskId { get; set; }
        public TaskModel Task { get; set; } = null!;
        public Guid UserId { get; set; }
        public UserModel User { get; set; } = null!;

        public HistoricTaskModel()
        {}
        public HistoricTaskModel(Guid orderId, EventEnum eventEnum, Guid userId)
        {
            Event = eventEnum;
            TaskId = orderId;
            UserId = userId;
        }

        public ResponseHistoricTaskDto Dtolize()
        {
            return new ResponseHistoricTaskDto
            {
                Id = Id,
                Event = Event,
                CreationDate = CreationDate,
                TaskId = TaskId,
                UserId = UserId
            };
        }
    }
}