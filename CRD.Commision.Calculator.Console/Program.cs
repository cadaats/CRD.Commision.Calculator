// See https://aka.ms/new-console-template for more information
using CRD.Commision.Calculator.Console;
using CRD.Commission.Calculator.Models;
using System.Diagnostics;

TestRunner testRunner = new TestRunner();
Stopwatch stopwatch = new Stopwatch();

long runTimeForLinear, runTimeForMaxParalleism, runTimeForParallelBatch, runTimeForSingleTrade = 0;
string testDataFile = "TestData/MultipleTrades_Large.json";

/// 
/// Run Single Trade
/// 
stopwatch.Start();
await testRunner.RunSingle(
    new TradeRequest(
        securityType: CRD.Commission.Calculator.Models.Enums.SecurityTypes.COM, 
        price: 100,
        quantity: 100_000, 
        tradeDate: DateTime.Now, 
        transactionType: CRD.Commission.Calculator.Models.Enums.TransactionType.BUY
    ));
stopwatch.Stop();
runTimeForSingleTrade = stopwatch.ElapsedMilliseconds;


///// 
///// Run Linear trades
///// 
stopwatch.Restart();
await testRunner.RunMultipleLinear(testDataFile);
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

Console.WriteLine($"Time taken to complete for Single Trade: {runTimeForSingleTrade} ms");
Console.WriteLine($"Time taken to complete by Linear  method: {runTimeForLinear} ms");
Console.WriteLine($"Time taken to complete in MAX parallelism: {runTimeForMaxParalleism} ms");
Console.WriteLine($"Time taken to complete in count of {batchCount} mode: {runTimeForParallelBatch} ms");

