using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Communication.Dto.Requests
{
    public class RequestTaskDto
    {
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int Quantity { get; set; }
        [Required]
        public Guid ProductId { get; set; }
    }
}