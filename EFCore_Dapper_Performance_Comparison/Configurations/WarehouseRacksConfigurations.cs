using EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore_Dapper_Performance_Comparison.Configurations.Products;
public class WarehouseRackConfigurations : IEntityTypeConfiguration<WarehouseRack>
{
    public void Configure(EntityTypeBuilder<WarehouseRack> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        var guidToStringConverter = new GuidToStringConverter();

        builder.Property(x => x.Id)
            .HasConversion(guidToStringConverter);

        builder.HasOne(x => x.WarehousesSize)
            .WithMany(x => x.WarehouseRacks)
            .HasForeignKey(x => x.WarehouseSizeId);

        builder.Property(x => x.WarehouseSizeId)
    .HasConversion(guidToStringConverter);
    }
}
