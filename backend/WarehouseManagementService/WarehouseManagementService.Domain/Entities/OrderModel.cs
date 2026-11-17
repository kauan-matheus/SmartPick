using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Requests;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Domain.Entities
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Destinatario { get; set; } = string.Empty;
        public string Transportadora { get; set; } = string.Empty;
        public DateTime DataColeta { get; set; } = DateTime.UtcNow;
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public StatusEnum Status { get; set; }

        public ResponseOrderDto ToDto()
        {
            return new ResponseOrderDto
            {
                Id = Id,
                Description = Description,
                Destinatario = Destinatario,
                Transportadora = Transportadora,
                DataColeta = DataColeta,
                TotalProdutos = Tasks.Sum(t => t.Quantity),
                Status = Status,
                Tasks = [.. Tasks.Select(t => t.ToDto())]
            };
        }
    }
}