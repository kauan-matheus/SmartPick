using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementService.Communication.Dto.Requests;
using WarehouseManagementService.Communication.Dto.Responses;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Interfaces;

namespace WarehouseManagementService.Application.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unit;

        public ProductService(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unit = unitOfWork;
        }

        public async Task<ServiceResponse<List<ResponseProductDto>>> Consultar()
        {
            try
            {
                var query = _repository.Consultar<ProductModel>();

                var products = await query.ToListAsync();

                return ServiceResponse<List<ResponseProductDto>>.Ok([.. products.Select(p => p.Dtolize())]);
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<ResponseProductDto>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<ResponseProductDto>> ConsultarPorId(Guid id)
        {
            try
            {
                var result =  await _repository.ConsultarPorId<ProductModel>(id);

                if (result == null)
                {
                    return ServiceResponse<ResponseProductDto>.BadRequest("Produto nao existe");
                }

                return ServiceResponse<ResponseProductDto>.Ok(result.Dtolize());
            }
            catch (Exception ex)
            {
                return ServiceResponse<ResponseProductDto>.Error(ex.Message);
            }
        }
        public async Task<ServiceResponse<ResponseProductDto>> Cadastrar(RequestProductDto product)
        {
            await _unit.BeginTransaction();

            try
            {
                var novo = new ProductModel
                {
                    Name = product.Name
                };

                await _repository.Cadastrar(novo);
                
                await _unit.Commit();
                await _unit.CommitTransaction();

                return ServiceResponse<ResponseProductDto>.Ok(novo.Dtolize());
            }
            catch (Exception ex)
            {
                await _unit.RollbackTransaction();
                return ServiceResponse<ResponseProductDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<ResponseProductDto>> Deletar(Guid id)
        {

            try
            {
                var existente = await _repository.ConsultarPorId<ProductModel>(id);

                if (existente == null)
                {
                    return ServiceResponse<ResponseProductDto>.BadRequest("Usuario nao existe");
                }

                _repository.Excluir(existente);
                var saved = await _unit.Commit();

                if (saved)
                {
                    return ServiceResponse<ResponseProductDto>.Ok(existente.Dtolize());
                }

                return ServiceResponse<ResponseProductDto>.Error("Nao foi possivel deletar esse pedido");
            }
            catch (Exception ex)
            {
                return ServiceResponse<ResponseProductDto>.Error(ex.Message);
            }
        }
    }
}