using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using Dapper;
using EFCore_Dapper_Performance_Comparison.Common.WarehouseConnectionFactory;
using EFCore_Dapper_Performance_Comparison.Models.Products;
using EFCore_Dapper_Performance_Comparison.Models.Transactions;
using EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks;
using EFCore_Dapper_Performance_Comparison.Models.WarehousesSize;
using EFCore_Dapper_Performance_Comparison.Models.Workers;
using System.Data;
using Warehouse.Infrastructure.Common;

namespace EFCore_Dapper_Performance_Comparison
{
    [Config(typeof(MyConfig))]
    [MemoryDiagnoser]
    public class Benchmarks
    {
        protected WarehouseDbContext _warehouseDbContext = null!;
        protected WarehouseConnectionFactory _connection = null!;
        protected IDbConnection _dbConnection = null!;
        private Random _random = null!;
        private WarehouseGenerator _warehouseGenerator = null!;
        protected Product _testProduct = null!;
        protected Worker _testWorker = null!;
        protected WarehouseSize _testWarehouseSize = null!;
        protected WarehouseRack _testWarehouseRack = null!;
        protected Transaction _testTransaction = null!;

        [GlobalSetup]
        public async Task Setup()
        {
            _random = new Random(420);
            _connection = new WarehouseConnectionFactory("Data Source=..\\..\\..\\Warehouse.db");
            _dbConnection = await _connection.CreateConnectionAsync();


            _warehouseGenerator = new WarehouseGenerator(_dbConnection, _random);
            await _warehouseGenerator.GenerateWarehouseDB(10);

            _testProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = "TestName",
                Type = ProductType.Two,
                Description = "DescriptionTest"
            };

            _testWorker = new Worker
            {
                Id = Guid.NewGuid(),
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Role = WorkerRole.Admin
            };

            _testWarehouseSize = new WarehouseSize
            {
                Id = Guid.NewGuid(),
                Name = "TestName",
                SectorNumber = 2,
                RackQuantity = 8
            };

            _testWarehouseRack = new WarehouseRack
            {
                Id = Guid.NewGuid(),
                Sector = 2,
                Rack = 7,
                Quantity = 200
            };

            _testTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Quantity = 200,
                Type = TransactionType.In,
                Date = DateTime.Now
            };

            await _dbConnection.ExecuteAsync("INSERT INTO Products (Id, Name, Type, Description) VALUES (@id, @name, @type, @description)", new
            {
                id = _testProduct.Id,
                name = _testProduct.Name,
                type = _testProduct.Type,
                description = _testProduct.Description
            });

            await _dbConnection.ExecuteAsync("INSERT INTO Workers (Id, FirstName, LastName, Role) VALUES (@Id, @FirstName, @LastName, @Role)", new
            {
                _testWorker.Id,
                _testWorker.FirstName,
                _testWorker.LastName,
                Role = _testWorker.Role.ToString()
            });

            await _dbConnection.ExecuteAsync("INSERT INTO WarehousesSize (Id, Name, SectorNumber, RackQuantity) VALUES (@id, @name, @sectorNumber, @rackQuantity)", new
            {
                id = _testWarehouseSize.Id,
                name = _testWarehouseSize.Name,
                sectorNumber = _testWarehouseSize.SectorNumber,
                rackQuantity = _testWarehouseSize.RackQuantity
            });

            await _dbConnection.ExecuteAsync("INSERT INTO WarehouseRacks (Id, Sector, Rack, Quantity, WarehouseSizeId) VALUES (@id, @sector, @rack, @quantity, @warehouseSizeId)", new
            {
                id = _testWarehouseRack.Id,
                sector = _testWarehouseRack.Sector,
                rack = _testWarehouseRack.Rack,
                quantity = _testWarehouseRack.Quantity,
                warehouseSizeId = _testWarehouseSize.Id
            });

            await _dbConnection.ExecuteAsync("INSERT INTO Transactions (Id, Quantity, Type, Date, ProductId, WarehouseRackId, WorkerId)"
                + "VALUES (@id, @quantity, @type, @date, @productId, @warehouseRackId, @workerId)", new
                {
                    id = _testTransaction.Id,
                    quantity = _testTransaction.Quantity,
                    type = _testTransaction.Type,
                    date = _testTransaction.Date,
                    productId = _testProduct.Id,
                    warehouseRackId = _testWarehouseRack.Id,
                    workerId = _testWorker.Id,
                });

            _warehouseDbContext = new();

            //CreateCasesBenchmark createCasesBenchmark = new CreateCasesBenchmark(_dbConnection, _warehouseDbContext);
            //createCasesBenchmark.GenerateData();
            //await createCasesBenchmark.EF_Core_AddWorkerAsync();
            //await createCasesBenchmark.EF_Core_AddAndSaveWorkerAsync();
            //await createCasesBenchmark.Dapper_AddWorkerAsync();

            //DeleteCasesBenchmark deleteCasesBenchmark = new DeleteCasesBenchmark(_dbConnection, _warehouseDbContext);
            //deleteCasesBenchmark.GenerateData();
            //await deleteCasesBenchmark.EF_Core_RemoveWorker();
            //await deleteCasesBenchmark.EF_Core_RemoveAndSaveWorker();
            //await deleteCasesBenchmark.Dapper_RemoveWorkerAsync();

            //UpdateCasesBenchmark updateCasesBenchmark = new UpdateCasesBenchmark(_dbConnection, _warehouseDbContext);
            //updateCasesBenchmark.GenerateData();
            //await updateCasesBenchmark.EF_Core_UpdateAndSaveWorker();
            //await updateCasesBenchmark.EF_Core_UpdateWorker();
            //await updateCasesBenchmark.Dapper_UpdateWorker();

            //ReadCasesBenchmark readBenchmark = new ReadCasesBenchmark(_dbConnection, _warehouseDbContext, _testWorker);
            //await readBenchmark.EF_FindAsync();
            //await readBenchmark.EF_FirstAsync();
            //await readBenchmark.EF_FirstOrDefaultAsync();
            //await readBenchmark.EF_SingleAsync();
            //await readBenchmark.EF_RAW();
            //await readBenchmark.Dapper_FirstAsync();
            //await readBenchmark.Dapper_FirstOrDefaultAsync();
            //await readBenchmark.Dapper_SingleAsync();

            //QuantitativeCasesBenchmark quantitativeCasesBenchmark = new QuantitativeCasesBenchmark(_dbConnection, _warehouseDbContext);
            //await quantitativeCasesBenchmark.EF_Core_AddWorkersRangeAndSaveAsync();
            //await quantitativeCasesBenchmark.Dapper_AddWorkersRangeAsync();
            //await quantitativeCasesBenchmark.EF_ToListAsyncLINQ();
            //await quantitativeCasesBenchmark.EF_ToListAsyncRaw();
            //await quantitativeCasesBenchmark.Dapper_ToListAsync();

            //ComplexCasesBenchmark complexCasesBenchmark = new ComplexCasesBenchmark(_dbConnection, _warehouseDbContext, _testTransaction, _testWarehouseRack);
            //await complexCasesBenchmark.EF_ComplexFirst();
            //await complexCasesBenchmark.EF_ComplexSecond();
            //await complexCasesBenchmark.EF_ComplexThird();
            //await complexCasesBenchmark.EF_ComplexFirstRaw();
            //await complexCasesBenchmark.EF_ComplexSecondRaw();
            //await complexCasesBenchmark.Ef_ComplexThirdRaw();
            //await complexCasesBenchmark.Dapper_ComplexFirst();
            //await complexCasesBenchmark.Dapper_ComplexSecond();
            //await complexCasesBenchmark.Dapper_ComplexThird();

            //ImprovementOfPerformanceCasesBenchmark improvementOfPerformanceCasesBenchmark = new ImprovementOfPerformanceCasesBenchmark(_dbConnection, _warehouseDbContext);
            //await improvementOfPerformanceCasesBenchmark.EF_WithAsNoTracking();
            //await improvementOfPerformanceCasesBenchmark.EF_WithoutAsNoTracking();
            //await improvementOfPerformanceCasesBenchmark.EF_WithSelectAll();
            //await improvementOfPerformanceCasesBenchmark.EF_WithoutSelectAll();
            //await improvementOfPerformanceCasesBenchmark.EF_WithPagination();
            //await improvementOfPerformanceCasesBenchmark.EF_WithoutPagination();
            //await improvementOfPerformanceCasesBenchmark.EF_WithRawQueries();
            //await improvementOfPerformanceCasesBenchmark.EF_WithoutRawQueries();
            //await improvementOfPerformanceCasesBenchmark.EF_WithCompiledQueries();
            //await improvementOfPerformanceCasesBenchmark.EF_WithoutCompiledQueries();
            //await improvementOfPerformanceCasesBenchmark.EF_WithSplitQueries();
            //await improvementOfPerformanceCasesBenchmark.EF_WithoutSplitQueries();

        }

        [GlobalCleanup]
        public async Task Cleanup()
        {
            await _warehouseGenerator.CleanupWarehouseDB();
        }

    }
    public class MyConfig : ManualConfig
    {
        public MyConfig()
        {
            //AddJob(BenchmarkDotNet.Jobs.Job.Default
            //    .WithToolchain(InProcessEmitToolchain.Instance));

            AddJob(Job.MediumRun
                .WithToolchain(InProcessNoEmitToolchain.Instance));
        }
    }
}
