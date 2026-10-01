using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Communication.Enums;

namespace WarehouseManagementService.Domain.Entities
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserTypeEnum Type { get; set; }
        public bool Active { get; set; } = true;

        public ResponseUserDto Dtolize()
        {
            return new ResponseUserDto
            {
                Id = Id,
                Nome = Name,
                Email = Email,
                Type = Type
            };
        }
    }
}