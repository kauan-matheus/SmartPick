using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Domain.Entities
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public StatusEnum Status { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public OrderModel Order { get; set; } = null!;
        public Guid ProductId { get; set; }
        public ProductModel Product { get; set; } = null!;

        public ResponseTaskDto Dtolize()
        {
            return new ResponseTaskDto
            {
                Id = Id,
                Description = Description,
                Status = Status,
                Quantity = Quantity,
                ProductId = ProductId
            };
        }
    }
}