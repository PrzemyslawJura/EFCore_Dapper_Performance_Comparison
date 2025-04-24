using EFCore_Dapper_Performance_Comparison.Models.Transactions;
using EFCore_Dapper_Performance_Comparison.Models.WarehousesSize;

namespace EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks;
public class WarehouseRack
{
    public Guid Id { get; set; }
    public int Sector { get; set; }
    public int Rack { get; set; }
    public int Quantity { get; set; }

    public ICollection<Transaction>? Transactions { get; private set; }
    public WarehouseSize WarehousesSize { get; set; }

    public Guid WarehouseSizeId { get; set; }

    public WarehouseRack(
        int sector,
        int rack,
        int quantity,
        Guid warehouseSizeId,
        Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Sector = sector;
        Rack = rack;
        Quantity = quantity;
        WarehouseSizeId = warehouseSizeId;
    }

    public WarehouseRack() { }
}
