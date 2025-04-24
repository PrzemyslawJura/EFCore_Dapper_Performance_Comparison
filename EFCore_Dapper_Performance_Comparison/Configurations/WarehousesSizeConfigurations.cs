using EFCore_Dapper_Performance_Comparison.Models.WarehousesSize;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore_Dapper_Performance_Comparison.Configurations.Products;
public class WarehouseSizeConfigurations : IEntityTypeConfiguration<WarehouseSize>
{
    public void Configure(EntityTypeBuilder<WarehouseSize> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        var guidToStringConverter = new GuidToStringConverter();

        builder.Property(x => x.Id)
            .HasConversion(guidToStringConverter);

        builder.Property(x => x.Name)
            .HasMaxLength(50);
    }
}
