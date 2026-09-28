using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseManagementService.Communication.Dto.Responses
{
    public class ResponseAuthDto
    {
        public string Token { get; set; } = string.Empty;
        public string ExpiresIn { get; set; } = string.Empty;
        public ResponseUserDto Usuario { get; set; } = new();
    }
}