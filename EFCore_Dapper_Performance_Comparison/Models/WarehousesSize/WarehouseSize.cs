using EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks;

namespace EFCore_Dapper_Performance_Comparison.Models.WarehousesSize;
public class WarehouseSize
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int SectorNumber { get; set; }
    public int RackQuantity { get; set; }

    public ICollection<WarehouseRack>? WarehouseRacks { get; private set; }

    public WarehouseSize(
        string name,
        int sectorNumber,
        int rackQuantity,
        Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        SectorNumber = sectorNumber;
        RackQuantity = rackQuantity;
    }

    public WarehouseSize() { }
}
