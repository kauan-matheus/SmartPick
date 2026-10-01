using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WarehouseManagementService.Communication.Dto.Requests;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Communication.Enums;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Interfaces;

namespace WarehouseManagementService.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;
        private readonly TokenService _tokenService;
        private readonly ILogger<AuthService> _logger;
        private readonly IUnitOfWork _unit;

        // ILogger injetado para registrar erros sem silenciá-los
        public AuthService(IUserRepository repository, TokenService tokenService, ILogger<AuthService> logger, IUnitOfWork unit)
        {
            _repository = repository;
            _tokenService = tokenService;
            _logger = logger;
            _unit = unit;
        }

        public async Task<ServiceResponse<ResponseAuthDto>> Login(RequestAuthLoginDto auth)
        {
            var usuario = await _repository.ConsultarUsuarioPorEmail(auth.Email);

            var senhaValida = usuario != null && await Task.Run(() => BCrypt.Net.BCrypt.Verify(auth.Password, usuario.PasswordHash));

            if (usuario == null || !senhaValida)
                return ServiceResponse<ResponseAuthDto>.Unauthorized("Email ou senha inválidos");

            if (!usuario.Active)
                return ServiceResponse<ResponseAuthDto>.Unauthorized("Usuário inativo");

            var token = _tokenService.GenerateToken(usuario);

            return ServiceResponse<ResponseAuthDto>.Ok(new ResponseAuthDto
            {
                Token = token,
                ExpiresIn = "8h",
                Usuario = new ResponseUserDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Name,
                    Email = usuario.Email,
                    Type = usuario.Type
                }
            });
        }

        public async Task<ServiceResponse<ResponseAuthDto>> RegisterEmployee(RequestAuthRegisterDto request)
        {
            var emailExiste = await _repository.ExisteUsuarioPorEmail(request.Email);
            if (emailExiste)
                return ServiceResponse<ResponseAuthDto>.BadRequest("Email já cadastrado");

            // BCrypt em background — HashPassword é pesado por design (segurança)
            var passwordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(request.Password));

            var usuario = new UserModel
            {
                Name = request.Nome,
                Email = request.Email,
                PasswordHash = passwordHash,
                Type = UserTypeEnum.EMPLOYEE,
                Active = true
            };

            try
            {
                // Cadastra usuário e profissional dentro de uma transaction (atômico)
                await _unit.BeginTransaction(); // começa transação

                await _repository.Cadastrar(usuario);

                await _unit.Commit();              // salva tudo
                await _unit.CommitTransaction();   // confirma
            }
            catch (Exception ex)
            {
                await _unit.RollbackTransaction();

                // Loga o erro real para debug, mas retorna mensagem genérica ao cliente
                _logger.LogError(ex, "Erro ao registrar usuário: {Email}", request.Email);
                return ServiceResponse<ResponseAuthDto>.Error("Não foi possível registrar o usuário");
            }

            // Após o cadastro o EF Core já preencheu usuario.Id com o valor gerado pelo banco
            // Só geramos o token aqui para garantir que o Id é válido
            var token = _tokenService.GenerateToken(usuario);

            return ServiceResponse<ResponseAuthDto>.Ok(new ResponseAuthDto
            {
                Token = token,
                ExpiresIn = "8h",
                Usuario = usuario.Dtolize()
            });
        }

        public async Task<ServiceResponse<ResponseAuthDto>> RegisterManager(RequestAuthRegisterDto request)
        {
            var emailExiste = await _repository.ExisteUsuarioPorEmail(request.Email);
            if (emailExiste)
                return ServiceResponse<ResponseAuthDto>.BadRequest("Email já cadastrado");

            var passwordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(request.Password));

            var usuario = new UserModel
            {
                Name = request.Nome,
                Email = request.Email,
                PasswordHash = passwordHash,
                Type = UserTypeEnum.MANAGER,
                Active = true
            };

            try
            {
                await _unit.BeginTransaction();

                await _repository.Cadastrar(usuario);

                await _unit.Commit();
                await _unit.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unit.RollbackTransaction();

                _logger.LogError(ex, "Erro ao registrar usuário: {Email}", request.Email);
                return ServiceResponse<ResponseAuthDto>.Error("Não foi possível registrar o usuário");
            }

            var token = _tokenService.GenerateToken(usuario);

            return ServiceResponse<ResponseAuthDto>.Ok(new ResponseAuthDto
            {
                Token = token,
                ExpiresIn = "8h",
                Usuario = usuario.Dtolize()
            });
        }
    }
}