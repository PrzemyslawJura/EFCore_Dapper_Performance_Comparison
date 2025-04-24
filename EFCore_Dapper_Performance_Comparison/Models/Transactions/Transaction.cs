using EFCore_Dapper_Performance_Comparison.Models.Products;
using EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks;
using EFCore_Dapper_Performance_Comparison.Models.Workers;

namespace EFCore_Dapper_Performance_Comparison.Models.Transactions;
public class Transaction
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }

    public Product Products { get; set; }
    public WarehouseRack WarehouseRacks { get; set; }
    public Worker Workers { get; set; }

    public Guid ProductId { get; set; }
    public Guid WarehouseRackId { get; set; }
    public Guid WorkerId { get; set; }


    public Transaction(
        int quantity,
        TransactionType type,
        DateTime date,
        Guid productId,
        Guid warehouseRackId,
        Guid workerId,
        Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Quantity = quantity;
        Type = type;
        Date = date;
        ProductId = productId;
        WarehouseRackId = warehouseRackId;
        WorkerId = workerId;
    }

    public Transaction() { }
}