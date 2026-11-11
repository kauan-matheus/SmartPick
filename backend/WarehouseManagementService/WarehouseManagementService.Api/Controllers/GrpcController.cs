using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Communication.Dto.Request;
using WarehouseManagementService.Application.Services.Grpc;

namespace WarehouseManagementService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GrpcController : ControllerBase
    {
        private readonly IGrpcService _service;

        public GrpcController(IGrpcService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetConnection()
        {
            var response = await _service.TesteGrpc();

            if (response.Success)
                return StatusCode(response.StatusCode, response.Data);

            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CallHeldKarp(RequestPathDto request)
        {
            var response = await _service.HeldKarp(request);

            if (response.Success)
                return StatusCode(response.StatusCode, response.Data);

            return StatusCode(response.StatusCode, response.Message);
        }
    }
}