using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WarehouseManagementService.Infra.Data.Data;

var builder = Host.CreateApplicationBuilder(
    new HostApplicationBuilderSettings
    {
        Args = args,
        EnvironmentName = Environments.Development
    }
);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var connectionString = builder.Configuration.GetConnectionString("Default");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("Connection string não encontrada!");
    return 1;
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, v => v.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
);

builder.Services.AddScoped<AppDbSeeder>();

using var host = builder.Build();
using var scope = host.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

try
{
    Console.WriteLine("Aplicando migrations...");
    await context.Database.MigrateAsync();

    Console.WriteLine("Executando seed...");
    await scope.ServiceProvider.GetRequiredService<AppDbSeeder>().SeedAsync();

    Console.WriteLine("Concluído com sucesso.");
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine($"Falha ao executar o Seeder: {ex}");
    return 1;
}