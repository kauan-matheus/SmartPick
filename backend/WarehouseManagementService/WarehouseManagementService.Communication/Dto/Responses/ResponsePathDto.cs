using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseManagementService.Communication.Dto.Responses
{
    public class ResponsePathDto
    {
        public ICollection<ResponseTaskInPathDto> LightTasks { get; set; } = new List<ResponseTaskInPathDto>();
        public ICollection<ResponseTaskInPathDto> HeavyTasks { get; set; } = new List<ResponseTaskInPathDto>();
    }
}