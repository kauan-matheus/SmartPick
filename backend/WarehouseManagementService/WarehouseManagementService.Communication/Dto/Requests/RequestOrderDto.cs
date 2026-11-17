using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Communication.Dto.Requests
{
    public class RequestOrderDto
    {
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Destinatario { get; set; } = string.Empty;
        [Required]
        public string Transportadora { get; set; } = string.Empty;
        [Required]
        public DateTime DataColeta { get; set; } = DateTime.UtcNow;
        public ICollection<RequestTaskDto> Tasks { get; set; } = new List<RequestTaskDto>();
    }
}