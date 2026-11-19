using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Communication.Dto.Request;
using WarehouseManagementService.Application.Services.Path;

namespace WarehouseManagementService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PathFindController : ControllerBase
    {
        private readonly IPathService _service;

        public PathFindController(IPathService service)
        {
            _service = service;
        }

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CallHeldKarp(Guid id)
        {
            var response = await _service.ConsultarPedido(id);

            if (response.Success)
                return StatusCode(response.StatusCode, response.Data);

            return StatusCode(response.StatusCode, response.Message);
        }
    }
}