using BenchmarkDotNet.Attributes;
using Dapper;
using EFCore_Dapper_Performance_Comparison.Models.Workers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Warehouse.Infrastructure.Common;

namespace EFCore_Dapper_Performance_Comparison.TestCases.BasicOperations
{
    public class CreateCasesBenchmark : Benchmarks
    {
        private Worker _efCoreTestWorker = new Worker();
        private Worker _dapperTestWorker = new Worker();

        public CreateCasesBenchmark(IDbConnection dbConnection, WarehouseDbContext warehouseDbContext)
        {
            _dbConnection = dbConnection;
            _warehouseDbContext = warehouseDbContext;
        }

        public CreateCasesBenchmark()
        { }

        [IterationSetup]
        public void GenerateData()
        {
            _efCoreTestWorker = new Worker
            {
                Id = Guid.NewGuid(),
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Role = WorkerRole.Admin
            };

            _dapperTestWorker = new Worker
            {
                Id = Guid.NewGuid(),
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Role = WorkerRole.Admin
            };
        }

        [Benchmark]
        public async Task EF_Core_AddWorkerAsync()
        {
            await _warehouseDbContext.Workers.AddAsync(_efCoreTestWorker);
        }

        [Benchmark]
        public async Task EF_Core_AddAndSaveWorkerAsync()
        {
            _warehouseDbContext.Workers.Add(_efCoreTestWorker);
            await _warehouseDbContext.SaveChangesAsync();
        }

        [Benchmark]
        public async Task Dapper_AddWorkerAsync()
        {
            await _dbConnection
                .ExecuteAsync("INSERT INTO Workers (Id, FirstName, LastName, Role) " +
                "VALUES (@Id, @FirstName, @LastName, @Role)", new
                {
                    _dapperTestWorker.Id,
                    _dapperTestWorker.FirstName,
                    _dapperTestWorker.LastName,
                    Role = _dapperTestWorker.Role.ToString()
                });
        }

    }
}
