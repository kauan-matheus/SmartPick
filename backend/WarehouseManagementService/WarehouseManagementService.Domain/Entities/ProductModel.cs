using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Domain.Entities
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ProductAccessEnum Access { get; set; }

        public ResponseProductDto ToDto()
        {
            return new ResponseProductDto
            {
                Id = Id,
                Name = Name,
                Access = Access
            };
        }
    }
}