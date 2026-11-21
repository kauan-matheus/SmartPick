using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Communication.Dto.Requests
{
    public class RequestRequestDto
    {
        [Required]
        public string Justification { get; set; } = string.Empty;
        [Required]
        public RequestObjectiveEnum Objective { get; set; }
        [Required]
        public Guid OrderId { get; set; }
    }
}