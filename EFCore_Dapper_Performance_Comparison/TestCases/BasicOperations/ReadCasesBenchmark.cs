using BenchmarkDotNet.Attributes;
using EFCore_Dapper_Performance_Comparison.Models.Workers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Warehouse.Infrastructure.Common;

namespace EFCore_Dapper_Performance_Comparison.TestCases.BasicOperations
{
    public class ReadCasesBenchmark : Benchmarks
    {
        public ReadCasesBenchmark(IDbConnection dbConnection, WarehouseDbContext warehouseDbContext, Worker testWorker)
        {
            _dbConnection = dbConnection;
            _warehouseDbContext = warehouseDbContext;
            _testWorker = testWorker;
        }

        public ReadCasesBenchmark()
        { }

        [Benchmark]
        public async Task<Worker?> EF_FirstAsNoTrackingAsync()
        {
            var result = await _warehouseDbContext.Workers.AsNoTracking().FirstAsync(x => x.Id == _testWorker.Id);
            return result;
        }

        [Benchmark]
        public async Task<Worker?> EF_FirstOrDefaultAsNoTrackingAsync()
        {
            var result = await _warehouseDbContext.Workers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == _testWorker.Id);
            return result;
        }

        [Benchmark]
        public async Task<Worker?> EF_SingleAsNoTrackingAsync()
        {
            var result = await _warehouseDbContext.Workers.AsNoTracking().SingleAsync(x => x.Id == _testWorker.Id);
            return result;
        }

        //[Benchmark]
        //public async Task<Worker?> EF_FindAsync()
        //{
        //    var result = await _warehouseDbContext.Workers.FindAsync(_testWorker.Id);
        //    return result;
        //}

        //[Benchmark]
        //public async Task<Worker?> EF_RAW()
        //{
        //    var result = await _warehouseDbContext.Workers.FromSqlRaw(
        //        "SELECT * FROM Workers WHERE Id = {0} LIMIT 1", _testWorker.Id.ToString()
        //        ).ToListAsync();
        //    return result[0];
        //}
        //[Benchmark]
        //public async Task<Worker?> Dapper_FirstAsync()
        //{
        //    var result = await _dbConnection.QueryFirstAsync<Worker?>(
        //        "SELECT * FROM Workers WHERE Id = @id LIMIT 1", new { id = _testWorker.Id }
        //    );
        //    return result;
        //}

        //[Benchmark]
        //public async Task<Worker?> Dapper_FirstOrDefaultAsync()
        //{
        //    var result = await _dbConnection.QueryFirstOrDefaultAsync<Worker?>(
        //        "SELECT * FROM Workers WHERE Id = @id LIMIT 1", new { id = _testWorker.Id }
        //    );
        //    return result;
        //}

        //[Benchmark]
        //public async Task<Worker?> Dapper_SingleAsync()
        //{
        //    var result = await _dbConnection.QuerySingleAsync<Worker?>(
        //        "SELECT * FROM Workers WHERE Id = @id LIMIT 2", new { id = _testWorker.Id }
        //    );
        //    return result;
        //}

    }
}
