using BenchmarkDotNet.Attributes;
using Dapper;
using EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Warehouse.Infrastructure.Common;
using Transaction = EFCore_Dapper_Performance_Comparison.Models.Transactions.Transaction;

namespace EFCore_Dapper_Performance_Comparison.TestCases.BasicOperations
{
    public class ComplexCasesBenchmark : Benchmarks
    {
        public ComplexCasesBenchmark(IDbConnection dbConnection, WarehouseDbContext warehouseDbContext, Transaction testTransaction, WarehouseRack testWarehouseRack)
        {
            _dbConnection = dbConnection;
            _warehouseDbContext = warehouseDbContext;
            _testTransaction = testTransaction;
            _testWarehouseRack = testWarehouseRack;
        }

        public ComplexCasesBenchmark()
        { }

        [Benchmark]
        public async Task EF_ComplexFirst()
        {
            var results = await _warehouseDbContext.Transactions
                .Include(transaction => transaction.Products)
                .Include(transaction => transaction.WarehouseRacks)
                     .ThenInclude(transaction => transaction.WarehousesSize)
                .Include(transaction => transaction.Workers)
                .Where(x => x.Id == _testTransaction.Id)
                .Where(x => x.Quantity > 100)
                .GroupBy(x => x.WarehouseRacks.Rack)
                .Select(x => new
                {
                    Rack = x.Count()
                })
                .ToListAsync();
        }

        [Benchmark]
        public async Task EF_ComplexSecond()
        {
            var results = await _warehouseDbContext.Transactions
                .Include(transaction => transaction.Products)
                .Include(transaction => transaction.WarehouseRacks)
                     .ThenInclude(transaction => transaction.WarehousesSize)
                .Include(transaction => transaction.Workers)
                .Select(x => new
                {
                    x.WorkerId,
                    x.Date,
                    Data = _warehouseDbContext.WarehouseRacks
                    .FirstOrDefault(x => x.Id == _testWarehouseRack.Id)
                    .Quantity
                    .CompareTo(200)
                    .ToString()
                })
                .ToListAsync();
        }

        [Benchmark]
        public async Task EF_ComplexThird()
        {
            var results = await _warehouseDbContext.Transactions
                .Include(transaction => transaction.Products)
                .Include(transaction => transaction.WarehouseRacks)
                     .ThenInclude(transaction => transaction.WarehousesSize)
                .Include(transaction => transaction.Workers)
                .Where(x => _warehouseDbContext.Transactions
                    .Where(i => i.Id == _testTransaction.Id)
                    .Select(i => i.Id)
                    .Distinct()
                    .Any())
                .ToListAsync();
        }

        [Benchmark]
        public async Task EF_ComplexFirstRaw()
        {
            string sql = @"
                SELECT t.Id AS Id, Date, ProductId, t.Quantity, Type, WorkerId, w.Id AS WarehouseRackId
                FROM Transactions AS t
                INNER JOIN WarehouseRacks AS w ON t.WarehouseRackId == w.Id
                WHERE (t.Id == @TestTransactionId) 
                      AND (t.Quantity > 100)";

            var param = new SqliteParameter("TestTransactionId", _testTransaction.Id.ToString());
            var results = await _warehouseDbContext.Transactions.FromSqlRaw(sql, param)
                .GroupBy(x => x.WarehouseRacks.Rack)
                .Select(x => new
                {
                    Rack = x.Count()
                })
                .ToListAsync();

            //foreach (var transaction in results)
            //{ Console.WriteLine(transaction.Rack); }
        }

        [Benchmark]
        public async Task EF_ComplexSecondRaw()
        {
            string sql = @"
                SELECT t.*
                FROM Transactions AS t
                INNER JOIN Products AS p ON t.ProductId = p.Id
                INNER JOIN WarehouseRacks AS w ON t.WarehouseRackId = w.Id
                INNER JOIN WarehousesSize AS w0 ON w.WarehouseSizeId = w0.Id
                INNER JOIN Workers AS w1 ON t.WorkerId = w1.Id";

            var param = new SqliteParameter("TestTransactionId", _testTransaction.Id.ToString());
            var results = await _warehouseDbContext.Transactions.FromSqlRaw(sql, param)
                .Select(x => new
                {
                    x.WorkerId,
                    x.Date,
                    Data = _warehouseDbContext.WarehouseRacks.FirstOrDefault(x => x.Id == _testWarehouseRack.Id).Quantity.CompareTo(200).ToString()
                })
                .ToListAsync();

            //foreach (var transaction in results)
            //{ Console.WriteLine(transaction.Date + "  asasda  " + transaction.Data); }
        }

        [Benchmark]
        public async Task Ef_ComplexThirdRaw()
        {
            string sql = @"
                SELECT t.*
                FROM Transactions AS t
                LEFT JOIN Products AS p ON t.ProductId = p.Id
                LEFT JOIN WarehouseRacks AS w ON t.WarehouseRackId = w.Id
                LEFT JOIN WarehousesSize AS w0 ON w.WarehouseSizeId = w0.Id
                LEFT JOIN Workers AS w1 ON t.WorkerId = w1.Id
                WHERE EXISTS (
                    SELECT 1 
                    FROM Transactions AS t0 
                    WHERE t0.Id = @TestTransactionId
                )";

            var param = new SqliteParameter("TestTransactionId", _testTransaction.Id.ToString());
            var results = await _warehouseDbContext.Transactions.FromSqlRaw(sql, param).ToListAsync();

            //foreach (var transaction in results)
            //{ Console.WriteLine(transaction.Id); }

        }

        [Benchmark]
        public async Task Dapper_ComplexFirst()
        {
            string sql = @"
                SELECT COUNT(*) AS Rack
                FROM Transactions AS t
                INNER JOIN WarehouseRacks AS w ON t.WarehouseRackId == w.Id
                WHERE (t.Id == @TestTransactionId) 
                      AND (t.Quantity > 100)
                GROUP BY w.Rack;";

            var result = await _dbConnection
                .QueryAsync(sql, new { TestTransactionId = _testTransaction.Id });

        }

        [Benchmark]
        public async Task Dapper_ComplexSecond()
        {
            string sql = @"
                WITH RackData AS (
                    SELECT Quantity
                    FROM Transactions AS t
                    WHERE t.Id = @TestTransactionId
                )
                SELECT 
                    t.WorkerId, 
                    t.Date, 
                    COALESCE(
                        CAST(
                            CASE 
                                WHEN (SELECT Quantity FROM RackData) = 200 THEN 0
                                WHEN (SELECT Quantity FROM RackData) > 200 THEN 1
                                WHEN (SELECT Quantity FROM RackData) < 200 THEN -1
                            END 
                        AS NVARCHAR), ''
                    ) AS Data
                FROM Transactions AS t;";

            var result = await _dbConnection
                .QueryAsync(sql, new { TestTransactionId = _testTransaction.Id });
        }

        [Benchmark]
        public async Task Dapper_ComplexThird()
        {
            string sql = @"
                SELECT t.*
                FROM Transactions AS t
                LEFT JOIN Products AS p ON t.ProductId = p.Id
                LEFT JOIN WarehouseRacks AS w ON t.WarehouseRackId = w.Id
                LEFT JOIN WarehousesSize AS w0 ON w.WarehouseSizeId = w0.Id
                LEFT JOIN Workers AS w1 ON t.WorkerId = w1.Id
                WHERE EXISTS (
                    SELECT 1 
                    FROM Transactions AS t0 
                    WHERE t0.Id = @TestTransactionId
                )";

            var result = await _dbConnection
                .QueryAsync(sql, new { TestTransactionId = _testTransaction.Id });
        }
    }
}
