using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseManagementService.Communication.Dto.Requests
{
    public class RequestProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}