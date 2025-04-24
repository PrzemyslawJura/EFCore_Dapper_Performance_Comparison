using BenchmarkDotNet.Attributes;
using Dapper;
using EFCore_Dapper_Performance_Comparison.Models.Workers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Warehouse.Infrastructure.Common;

namespace EFCore_Dapper_Performance_Comparison.TestCases.BasicOperations
{
    public class UpdateCasesBenchmark : Benchmarks
    {
        private Worker[] _testWorkers = new Worker[3];
        private Worker[] _updateWorkers = new Worker[3];
        public UpdateCasesBenchmark(IDbConnection dbConnection, WarehouseDbContext warehouseDbContext)
        {
            _dbConnection = dbConnection;
            _warehouseDbContext = warehouseDbContext;
        }

        public UpdateCasesBenchmark()
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

                _updateWorkers[i] = new Worker
                {
                    Id = _testWorkers[i].Id,
                    FirstName = "UpdateTestFirstName",
                    LastName = "UpdateTestLastName",
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
        public Task EF_Core_UpdateAndSaveWorker()
        {
            _warehouseDbContext.Update(_updateWorkers[0]);
            _warehouseDbContext.SaveChangesAsync();

            return Task.CompletedTask;
        }

        [Benchmark]
        public Task EF_Core_UpdateWorker()
        {
            _warehouseDbContext.Update(_updateWorkers[1]);

            return Task.CompletedTask;
        }

        [Benchmark]
        public Task Dapper_UpdateWorker()
        {
            _dbConnection
                .ExecuteAsync("UPDATE Workers SET FirstName = @firstName, LastName = @lastName, Role = @role WHERE Id = @id", new
                {
                    id = _updateWorkers[2].Id,
                    firstName = _updateWorkers[2].FirstName,
                    lastName = _updateWorkers[2].LastName,
                    role = _updateWorkers[2].Role.ToString()
                });
            return Task.CompletedTask;
        }

    }
}
