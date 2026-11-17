using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseManagementService.Communication.Dto.Responses
{
    public class ResponsePathDto
    {
        public ICollection<ResponseTaskDto> LightTasks { get; set; } = new List<ResponseTaskDto>();
        public ICollection<ResponseTaskDto> HeavyTasks { get; set; } = new List<ResponseTaskDto>();
    }
}