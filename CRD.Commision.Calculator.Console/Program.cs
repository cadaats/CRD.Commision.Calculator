// See https://aka.ms/new-console-template for more information
using CRD.Commision.Calculator.Console;
using System.Diagnostics;

TestRunner testRunner = new TestRunner();
Stopwatch stopwatch = new Stopwatch();

long runTimeForLinear, runTimeForMaxParalleism, runTimeForParallelBatch = 0;
string testDataFile = "Tests/MultipleTrades_Large.json";

///// 
///// Run Linear trades
///// 
stopwatch.Restart();
await testRunner.RunLinear(testDataFile);
stopwatch.Stop();
runTimeForLinear = stopwatch.ElapsedMilliseconds;

///// 
///// Run multiple trades in parallel
///// 

stopwatch.Restart();
await testRunner.RunMultipleTradesWithMaxParallelism(testDataFile);
stopwatch.Stop();
runTimeForMaxParalleism = stopwatch.ElapsedMilliseconds;

/// 
/// Run multiple trades in parallel - by batchsize
/// 

stopwatch.Restart();
int batchCount = 10;
await testRunner.RunMultipleTradesInParallel(testDataFile, batchCount);
stopwatch.Stop();
runTimeForParallelBatch = stopwatch.ElapsedMilliseconds;

Console.WriteLine($"Time taken to complete by Linear  method: {runTimeForLinear} ms");
Console.WriteLine($"Time taken to complete in MAX parallelism: {runTimeForMaxParalleism} ms");
Console.WriteLine($"Time taken to complete in count of {batchCount} mode: {runTimeForParallelBatch} ms");

