using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Warehouse.Infrastructure.Common;

namespace EFCore_Dapper_Performance_Comparison.TestCases.BasicOperations
{
    public class ImprovementOfPerformanceCasesBenchmark : Benchmarks
    {
        public ImprovementOfPerformanceCasesBenchmark(IDbConnection dbConnection, WarehouseDbContext warehouseDbContext)
        {
            _dbConnection = dbConnection;
            _warehouseDbContext = warehouseDbContext;
        }
        public ImprovementOfPerformanceCasesBenchmark()
        { }

        //[Benchmark]
        //public async Task EF_WithoutAsNoTracking()
        //{
        //    await _warehouseDbContext.Workers.AsNoTracking().ToListAsync();
        //}

        //[Benchmark]
        //public async Task EF_WithAsNoTracking()
        //{
        //    var results = await _warehouseDbContext.Workers.ToListAsync();
        //}

        //[Benchmark]
        //public async Task EF_WithSelectAll()
        //{
        //    await _warehouseDbContext.Workers
        //        .Select(x => x.Id)
        //        .ToListAsync();
        //}

        //[Benchmark]
        //public async Task EF_WithoutSelectAll()
        //{
        //    var results = await _warehouseDbContext.Workers.ToListAsync();
        //}

        //[Benchmark]
        //public async Task EF_WithPagination()
        //{
        //    await _warehouseDbContext.Workers.Skip(10).Take(5).ToListAsync();
        //}

        [Benchmark]
        public async Task EF_WithoutPagination()
        {
            await _warehouseDbContext.Workers.ToListAsync();
        }

        //[Benchmark]
        //public async Task EF_WithoutRawQueries()
        //{
        //    await _warehouseDbContext.Workers.ToListAsync();
        //}

        //[Benchmark]
        //public async Task EF_WithRawQueries()
        //{
        //    await _warehouseDbContext.Workers
        //        .FromSqlRaw("SELECT * FROM Workers").ToListAsync();
        //}

        //[Benchmark]
        //public async Task EF_WithoutCompiledQueries()
        //{
        //    var results = await _warehouseDbContext.Workers.ToListAsync();
        //}

        //static readonly Func<WarehouseDbContext, bool, List<Worker>> _compiledQuery =
        //    EF.CompileQuery((WarehouseDbContext context, bool isActive) =>
        //    context.Workers.ToList());

        //[Benchmark]
        //public async Task EF_WithCompiledQueries()
        //{
        //    var results = _compiledQuery(_warehouseDbContext, true);
        //}

        //[Benchmark]
        //public async Task EF_WithoutSplitQueries()
        //{
        //    var results = await _warehouseDbContext.Transactions
        //        .Include(transaction => transaction.Products)
        //        .Include(transaction => transaction.WarehouseRacks)
        //             .ThenInclude(transaction => transaction.WarehousesSize)
        //        .Include(transaction => transaction.Workers)
        //        .Select(x => new
        //        {
        //            x.WorkerId,
        //            x.Date,
        //            Data = _warehouseDbContext.WarehouseRacks.FirstOrDefault(x => x.Id == _testWarehouseRack.Id).Quantity.CompareTo(200).ToString()
        //        })
        //        .ToListAsync();

        //}

        //[Benchmark]
        //public async Task EF_WithSplitQueries()
        //{
        //    var results = await _warehouseDbContext.Transactions
        //        .Include(transaction => transaction.Products)
        //        .Include(transaction => transaction.WarehouseRacks)
        //             .ThenInclude(transaction => transaction.WarehousesSize)
        //        .Include(transaction => transaction.Workers)
        //        .Select(x => new
        //        {
        //            x.WorkerId,
        //            x.Date,
        //            Data = _warehouseDbContext.WarehouseRacks
        //            .FirstOrDefault(x => x.Id == _testWarehouseRack.Id)
        //            .Quantity.CompareTo(200).ToString()
        //        })
        //        .AsSplitQuery()
        //        .ToListAsync();

        //}


    }
}
