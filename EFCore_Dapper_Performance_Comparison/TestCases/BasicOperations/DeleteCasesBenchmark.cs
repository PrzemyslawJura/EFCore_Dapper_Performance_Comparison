using BenchmarkDotNet.Attributes;
using Dapper;
using EFCore_Dapper_Performance_Comparison.Models.Workers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Warehouse.Infrastructure.Common;

namespace EFCore_Dapper_Performance_Comparison.TestCases.BasicOperations
{
    public class DeleteCasesBenchmark : Benchmarks
    {
        private Worker[] _testWorkers = new Worker[3];

        public DeleteCasesBenchmark(IDbConnection dbConnection, WarehouseDbContext warehouseDbContext)
        {
            _dbConnection = dbConnection;
            _warehouseDbContext = warehouseDbContext;
        }

        public DeleteCasesBenchmark()
        { }

        [IterationSetup]
        public void GenerateData()
        {
            for (int i = 0; i < 3; i++)
            {
                _testWorkers[i] = new Worker
                {
                    Id = Guid.NewGuid(),
                    FirstName = "TestFirstName",
                    LastName = "TestLastName",
                    Role = WorkerRole.Admin
                };

                _dbConnection.ExecuteAsync("INSERT INTO Workers (Id, FirstName, LastName, Role) VALUES (@Id, @FirstName, @LastName, @Role)", new
                {
                    _testWorkers[i].Id,
                    _testWorkers[i].FirstName,
                    _testWorkers[i].LastName,
                    Role = _testWorkers[i].Role.ToString()
                });
            }
        }

        [Benchmark]
        public Task EF_Core_RemoveAndSaveWorker()
        {
            _warehouseDbContext.Remove(_testWorkers[0]);
            _warehouseDbContext.SaveChangesAsync();

            return Task.CompletedTask;
        }

        [Benchmark]
        public Task EF_Core_RemoveWorker()
        {
            _warehouseDbContext.Remove(_testWorkers[1]);

            return Task.CompletedTask;
        }

        [Benchmark]
        public Task Dapper_RemoveWorkerAsync()
        {
            _dbConnection.ExecuteAsync("DELETE FROM Workers WHERE Id = @id", new { id = _testWorkers[2].Id });
            return Task.CompletedTask;
        }
    }
}
