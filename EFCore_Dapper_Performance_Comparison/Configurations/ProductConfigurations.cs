using EFCore_Dapper_Performance_Comparison.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore_Dapper_Performance_Comparison.Configurations.Products;
public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        var guidToStringConverter = new GuidToStringConverter();

        builder.Property(x => x.Id)
            .HasConversion(guidToStringConverter);

        builder.Property(x => x.Name)
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .HasMaxLength(250);

        builder.Property(x => x.Type)
            .HasConversion<string>();
    }
}
