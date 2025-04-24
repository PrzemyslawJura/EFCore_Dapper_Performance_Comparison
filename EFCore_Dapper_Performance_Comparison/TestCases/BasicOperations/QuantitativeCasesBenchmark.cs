using BenchmarkDotNet.Attributes;
using Bogus;
using Dapper;
using EFCore_Dapper_Performance_Comparison.Models.Workers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Warehouse.Infrastructure.Common;

namespace EFCore_Dapper_Performance_Comparison.TestCases.BasicOperations
{
    public class QuantitativeCasesBenchmark : Benchmarks
    {
        private readonly Faker<Worker> _workerGenerator = new Faker<Worker>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.FirstName, f => f.Person.FirstName)
            .RuleFor(p => p.LastName, f => f.Person.LastName)
            .RuleFor(p => p.Role, f => f.PickRandom<WorkerRole>());
        List<Worker> _efWorkers = new List<Worker>();
        List<Worker> _dapperWorkers = new List<Worker>();
        public QuantitativeCasesBenchmark(IDbConnection dbConnection, WarehouseDbContext warehouseDbContext)
        {
            _dbConnection = dbConnection;
            _warehouseDbContext = warehouseDbContext;
            _efWorkers = _workerGenerator.Generate(100);
            _dapperWorkers = _workerGenerator.Generate(100);
        }

        public QuantitativeCasesBenchmark()
        { }

        [IterationSetup]
        public void GenerateData()
        {
            _efWorkers = _workerGenerator.Generate(100);
            _dapperWorkers = _workerGenerator.Generate(100);
        }

        [Benchmark]
        public async Task EF_Core_AddWorkersRangeAndSaveAsync()
        {
            await _warehouseDbContext.Workers.AddRangeAsync(_efWorkers);
            await _warehouseDbContext.SaveChangesAsync();
        }

        [Benchmark]
        public async Task Dapper_AddWorkersRangeAsync()
        {
            await _dbConnection
                .ExecuteAsync("INSERT INTO Workers (Id, FirstName, LastName, Role) " +
                "VALUES (@Id, @FirstName, @LastName, @Role)",
                _dapperWorkers);

        }

        [Benchmark]
        public async Task EF_ToListAsyncLINQ()
        {
            var results = await _warehouseDbContext.Workers.ToListAsync();
        }

        [Benchmark]
        public async Task EF_ToListAsyncRaw()
        {
            var results = await _warehouseDbContext.Workers.FromSqlRaw(
                "SELECT * FROM Workers").ToListAsync();
        }

        [Benchmark]
        public async Task Dapper_ToListAsync()
        {
            var results = await _dbConnection.QueryAsync(
                "SELECT * FROM Workers");
        }

    }
}
