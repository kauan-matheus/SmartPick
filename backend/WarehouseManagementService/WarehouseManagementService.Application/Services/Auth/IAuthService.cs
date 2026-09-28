using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementService.Communication.Dto.Requests;
using WarehouseManagementService.Communication.Dto.Responses;

namespace WarehouseManagementService.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<ServiceResponse<ResponseAuthDto>> Login(RequestAuthLoginDto auth);
        Task<ServiceResponse<ResponseAuthDto>> RegisterEmployee(RequestAuthRegisterDto request);
        Task<ServiceResponse<ResponseAuthDto>> RegisterManager(RequestAuthRegisterDto request);
    }
}