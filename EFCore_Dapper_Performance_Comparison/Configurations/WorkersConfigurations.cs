using EFCore_Dapper_Performance_Comparison.Models.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore_Dapper_Performance_Comparison.Configurations.Products;
public class WorkerConfigurations : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        var guidToStringConverter = new GuidToStringConverter();

        builder.Property(x => x.Id)
            .HasConversion(guidToStringConverter);

        builder.Property(x => x.FirstName)
            .HasMaxLength(50);

        builder.Property(x => x.LastName)
            .HasMaxLength(50);

        builder.Property(x => x.Role)
            .HasConversion<string>();
    }
}
