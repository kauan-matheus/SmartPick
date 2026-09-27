using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagementService.Application.Services.Grpc;
using WarehouseManagementService.Application.Services.Order;
using WarehouseManagementService.Application.Services.Product;
using WarehouseManagementService.Application.Services.User;
using WarehouseManagementService.Domain.Interfaces;
using WarehouseManagementService.Infra.Data.Data;
using WarehouseManagementService.Infra.Data.Repository;
using WarehouseManagementService.Infra.Data.UnitOfWork;

namespace WarehouseManagementService.Infra.IOC.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString, v => v.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IGrpcService, GrpcService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
    }
}