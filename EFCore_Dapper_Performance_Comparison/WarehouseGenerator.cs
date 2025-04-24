using Bogus;
using Dapper;
using EFCore_Dapper_Performance_Comparison.Models.Products;
using EFCore_Dapper_Performance_Comparison.Models.Transactions;
using EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks;
using EFCore_Dapper_Performance_Comparison.Models.WarehousesSize;
using EFCore_Dapper_Performance_Comparison.Models.Workers;
using System.Data;

namespace EFCore_Dapper_Performance_Comparison
{
    public class WarehouseGenerator
    {
        private readonly IDbConnection _dbConnection;

        private readonly Faker<Product> _productGenerator = new Faker<Product>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Type, f => f.PickRandom<ProductType>())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence());

        private readonly Faker<Worker> _workerGenerator = new Faker<Worker>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.FirstName, f => f.Person.FirstName)
            .RuleFor(p => p.LastName, f => f.Person.LastName)
            .RuleFor(p => p.Role, f => f.PickRandom<WorkerRole>());

        private readonly Faker<WarehouseSize> _warehouseSizeGenerator = new Faker<WarehouseSize>()
            .RuleFor(w => w.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(w => w.SectorNumber, f => f.Random.Int(1, 10))
            .RuleFor(w => w.RackQuantity, f => f.Random.Int(1, 50));

        private readonly Faker<WarehouseRack> _warehouseRackGenerator = new Faker<WarehouseRack>()
            .RuleFor(w => w.Id, f => Guid.NewGuid())
            .RuleFor(w => w.Sector, f => f.Random.Int(1, 10))
            .RuleFor(w => w.Rack, f => f.Random.Int(1, 50))
            .RuleFor(w => w.Quantity, f => f.Random.Int(0, 1000));

        private readonly Faker<Models.Transactions.Transaction> _transactionGenerator = new Faker<Models.Transactions.Transaction>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Quantity, f => f.Random.Int(0, 1000))
            .RuleFor(p => p.Type, f => f.PickRandom<TransactionType>())
            .RuleFor(p => p.Date, f => f.Date.Past());

        public WarehouseGenerator(IDbConnection dbConnection, Random random)
        {
            Randomizer.Seed = random;
            _dbConnection = dbConnection;
        }

        public async Task GenerateWarehouseDB(int count)
        {
            var products = _productGenerator.Generate(count);
            var workers = _workerGenerator.Generate(count);
            var warehousesSize = _warehouseSizeGenerator.Generate(count);
            var warehouseRacks = _warehouseRackGenerator.Generate(count);
            var transactions = _transactionGenerator.Generate(count);

            var insertProduct = "INSERT INTO Products (Id, Name, Type, Description) VALUES (@id, @name, @type, @description)";
            var insertWorker = "INSERT INTO Workers (Id, FirstName, LastName, Role) VALUES (@id, @firstName, @lastName, @role)";
            var insertWarehouseSize = "INSERT INTO WarehousesSize (Id, Name, SectorNumber, RackQuantity) VALUES (@id, @name, @sectorNumber, @rackQuantity)";
            var insertWarehouseRack = "INSERT INTO WarehouseRacks (Id, Sector, Rack, Quantity, WarehouseSizeId) VALUES (@id, @sector, @rack, @quantity, @warehouseSizeId)";
            var insertTransaction = "INSERT INTO Transactions (Id, Quantity, Type, Date, ProductId, WarehouseRackId, WorkerId)"
                + "VALUES (@id, @quantity, @type, @date, @productId, @warehouseRackId, @workerId)";

            using (var connection = _dbConnection)
            {
                for (var i = 0; i < count; i++)
                {
                    await connection.ExecuteAsync(insertProduct, new
                    {
                        id = products[i].Id,
                        name = products[i].Name,
                        type = products[i].Type,
                        description = products[i].Description
                    });

                    await connection.ExecuteAsync(insertWorker, new
                    {
                        id = workers[i].Id,
                        firstName = workers[i].FirstName,
                        lastName = workers[i].LastName,
                        role = workers[i].Role
                    });

                    await connection.ExecuteAsync(insertWarehouseSize, new
                    {
                        id = warehousesSize[i].Id,
                        name = warehousesSize[i].Name,
                        sectorNumber = warehousesSize[i].SectorNumber,
                        rackQuantity = warehousesSize[i].RackQuantity
                    });

                    await connection.ExecuteAsync(insertWarehouseRack, new
                    {
                        id = warehouseRacks[i].Id,
                        sector = warehouseRacks[i].Sector,
                        rack = warehouseRacks[i].Rack,
                        quantity = warehouseRacks[i].Quantity,
                        warehouseSizeId = warehousesSize[i].Id
                    });

                    await connection.ExecuteAsync(insertTransaction, new
                    {
                        id = transactions[i].Id,
                        quantity = transactions[i].Quantity,
                        type = transactions[i].Type,
                        date = transactions[i].Date,
                        productId = products[i].Id,
                        warehouseRackId = warehouseRacks[i].Id,
                        workerId = workers[i].Id,
                    });
                }
            }
        }

        public async Task CleanupWarehouseDB()
        {
            var deleteTransaction = "DELETE FROM Transactions";
            var deleteWarehouseRack = "DELETE FROM WarehouseRacks";
            var deleteProduct = "DELETE FROM Products";
            var deleteWorker = "DELETE FROM Workers";
            var deleteWarehouseSize = "DELETE FROM WarehousesSize";

            using (var connection = _dbConnection)
            {
                await connection.ExecuteAsync(deleteTransaction);
                await connection.ExecuteAsync(deleteWarehouseRack);
                await connection.ExecuteAsync(deleteProduct);
                await connection.ExecuteAsync(deleteWorker);
                await connection.ExecuteAsync(deleteWarehouseSize);
            }
        }
    }
}
