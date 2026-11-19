using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WarehouseManagementService.Domain.Entities;
using WarehouseManagementService.Domain.Models;

namespace WarehouseManagementService.Infra.Data.Configurations
{
    internal class MapConfiguration : IEntityTypeConfiguration<MapModel>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<MapModel> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired();
            var converter = new ValueConverter<MapClass, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<MapClass>(v, (JsonSerializerOptions?)null)!
            );

            builder.Property(x => x.Data)
                .HasColumnType("jsonb")
                .HasConversion(converter);
        }
    }
}
