using EFCore_Dapper_Performance_Comparison.Common.IUnitOfWork;
using EFCore_Dapper_Performance_Comparison.Models.Products;
using EFCore_Dapper_Performance_Comparison.Models.Transactions;
using EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks;
using EFCore_Dapper_Performance_Comparison.Models.WarehousesSize;
using EFCore_Dapper_Performance_Comparison.Models.Workers;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Warehouse.Infrastructure.Common;
public class WarehouseDbContext : DbContext, IUnitOfWork
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<WarehouseRack> WarehouseRacks { get; set; }
    public DbSet<WarehouseSize> WarehousesSize { get; set; }
    public DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //optionsBuilder.LogTo(Console.WriteLine);
        optionsBuilder.UseSqlite("Data Source=..\\..\\..\\Warehouse.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public async Task CommitChangesAsync()
    {
        await SaveChangesAsync();
    }
}
