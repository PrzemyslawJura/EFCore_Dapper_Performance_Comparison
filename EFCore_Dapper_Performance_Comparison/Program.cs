using BenchmarkDotNet.Running;
using EFCore_Dapper_Performance_Comparison;
using EFCore_Dapper_Performance_Comparison.TestCases.BasicOperations;

Benchmarks benchmark = new Benchmarks();
benchmark.Setup();
benchmark.Cleanup();

//BenchmarkRunner.Run<CreateCasesBenchmark>();
//BenchmarkRunner.Run<DeleteCasesBenchmark>();
//BenchmarkRunner.Run<UpdateCasesBenchmark>();
//BenchmarkRunner.Run<ReadCasesBenchmark>();
//BenchmarkRunner.Run<QuantitativeCasesBenchmark>();
//BenchmarkRunner.Run<ComplexCasesBenchmark>();
//BenchmarkRunner.Run<ImprovementOfPerformanceCasesBenchmark>();