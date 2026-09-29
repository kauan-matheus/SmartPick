using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementService.Application.Services.Auth;
using WarehouseManagementService.Communication.Dto.Requests;
using WarehouseManagementService.Communication.Dto.Responses;

namespace WarehouseManagementService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("/login")]
        [AllowAnonymous] // Login precisa ficar público para emitir o primeiro JWT
        [ProducesResponseType(typeof(ResponseAuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] RequestAuthLoginDto auth)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.Login(auth);

            if (response.Success)
                return StatusCode(response.StatusCode, response.Data);

            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPost("/auth/register-funcionario")]
        [AllowAnonymous] // Registro padrão é público e cria usuário profissional
        [ProducesResponseType(typeof(ResponseAuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterEmployee([FromBody] RequestAuthRegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.RegisterEmployee(request);

            if (response.Success)
                return StatusCode(response.StatusCode, response.Data);

            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPost("/auth/register-gerente")]
        [AllowAnonymous] // Registro padrão é público e cria usuário profissional
        [ProducesResponseType(typeof(ResponseAuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterManager([FromBody] RequestAuthRegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.RegisterManager(request);

            if (response.Success)
                return StatusCode(response.StatusCode, response.Data);

            return StatusCode(response.StatusCode, response.Message);
        }
    }
}