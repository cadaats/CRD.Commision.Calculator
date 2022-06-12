using System;
using CRD.Commision.Calculator.ConsoleApp;
using CRD.Commission.Calculator.Models;
using System.Diagnostics;

namespace CRD.Commision.Calculator.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            TestRunner testRunner = new TestRunner();
            
            long runTimeForLinear, runTimeForMaxParalleism, runTimeForParallelBatch, runTimeForSingleTrade = 0;
            string testDataFile = "TestData/MultipleTrades_Large.json";
            ///// 
            ///// Run Linear trades
            ///// 
            Stopwatch stopwatch = Stopwatch.StartNew();
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

            /// 
            /// Run Single Trade
            /// 
            stopwatch.Restart();
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

            System.Console.WriteLine($"Time taken to complete for Single Trade: {runTimeForSingleTrade} ms");
            System.Console.WriteLine($"Time taken to complete by Linear  method: {runTimeForLinear} ms");
            System.Console.WriteLine($"Time taken to complete in MAX parallelism: {runTimeForMaxParalleism} ms");
            System.Console.WriteLine($"Time taken to complete in count of {batchCount} mode: {runTimeForParallelBatch} ms");
        }
    }
}
