// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using CRD.Commision.Calculator.Console;
using System.Diagnostics;

BenchmarkRunner.Run<TestRunner>();

//TestRunner testRunner = new TestRunner();
//Stopwatch stopwatch = new Stopwatch();

/////// 
/////// Run single trade
/////// 
////await testRunner.RunSingleTrade();

/////// 
/////// Run multiple trades in parallel
/////// 
////stopwatch.Restart();
//await testRunner.RunMultipleTradesWithMaxParallelism("Tests/MultipleTrades_Test2.json");
////stopwatch.Stop();
////Console.WriteLine($"Time taken to complete in MAX parallelism is {stopwatch.ElapsedMilliseconds} ms");

///// 
///// Run multiple trades in parallel - batchsize
///// 
////stopwatch.Restart();
//int batchCount = 10;
//await testRunner.RunMultipleTradesInParallel("Tests/MultipleTrades_Test2.json", batchCount);
////stopwatch.Stop();
////Console.WriteLine($"Time taken to complete in count of {batchCount} in parallel is {stopwatch.ElapsedMilliseconds} ms");

