using EFCore_Dapper_Performance_Comparison.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore_Dapper_Performance_Comparison.Configurations.Products;
public class TransactionConfigurations : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        var guidToStringConverter = new GuidToStringConverter();

        builder.Property(x => x.Id)
            .HasConversion(guidToStringConverter);

        builder.Property(x => x.Type)
            .HasConversion<string>();

        builder.HasOne(x => x.Products)
              .WithMany(x => x.Transactions)
              .HasForeignKey(x => x.ProductId);

        builder.HasOne(x => x.WarehouseRacks)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.WarehouseRackId);

        builder.HasOne(x => x.Workers)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.WorkerId);

        builder.Property(x => x.ProductId)
            .HasConversion(guidToStringConverter);

        builder.Property(x => x.WarehouseRackId)
            .HasConversion(guidToStringConverter);

        builder.Property(x => x.WorkerId)
            .HasConversion(guidToStringConverter);
    }

}
