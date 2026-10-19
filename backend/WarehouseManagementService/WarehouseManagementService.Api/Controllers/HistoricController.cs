using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementService.Application.Services.Historic;

namespace WarehouseManagementService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HistoricController : ControllerBase
    {
        private readonly IHistoricService _service;

        public HistoricController(IHistoricService service)
        {
            _service = service;
        }

        [HttpGet("HistoricoPedidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPedidos()
        {
            var response = await _service.ConsultarHistoricoPedidos();

            if (response.Success)
                return StatusCode(response.StatusCode, response.Data);

            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpGet("HistoricoTarefas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTarefas()
        {
            var response = await _service.ConsultarHistoricoTarefas();

            if (response.Success)
                return StatusCode(response.StatusCode, response.Data);

            return StatusCode(response.StatusCode, response.Message);
        }
    }
}