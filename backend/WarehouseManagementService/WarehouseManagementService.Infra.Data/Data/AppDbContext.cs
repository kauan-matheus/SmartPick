using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementService.Domain.Entities;

namespace WarehouseManagementService.Infra.Data.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        // Colunas que serao definidas no banco de dados
        public DbSet<UserModel> Usuarios { get; set; }
        public DbSet<OrderModel> Pedidos { get; set; }
        public DbSet<TaskModel> Tarefas { get; set; }
        public DbSet<ProductModel> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.Product)
                .WithMany()
                .HasForeignKey(t => t.ProductId);
            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.Order)
                .WithMany(o => o.Tasks)
                .HasForeignKey(t => t.OrderId);
        }
    }
}