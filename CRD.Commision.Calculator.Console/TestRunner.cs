using BenchmarkDotNet.Attributes;
using CRD.Commission.Calculator;
using CRD.Commission.Calculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRD.Commision.Calculator.Console
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class TestRunner
    {
        FeeCalculatorService feeCalculator = new FeeCalculatorService();
        JsonReader reader = new JsonReader();

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            //WriteIndented = true,
        };

        [Benchmark]
        public async Task RunLinear()
        {
            //var comm = await feeCalculator.CalculateCommission(new TradeRequest()
            //{
            //    SecurityType = "COM",
            //    Price = 100,
            //    Quantity = 10000,
            //    TransactionType = CRD.Commission.Calculator.Models.Enums.TradeSide.BUY
            //});

            //System.Console.WriteLine(JsonSerializer.Serialize(comm, options));

            string filePath = "Tests/MultipleTrades_Test2.json";
            List<TradeRequest>? tradeRequests = reader.GetTradesFromJson(filePath);
            List<TradeResponse>? tradeResponses = new List<TradeResponse>();

            tradeRequests.ForEach(async t => 
            {
                tradeResponses.Add(await feeCalculator.CalculateCommission(t));
            });

            if (tradeResponses != null)
            {
                //System.Console.WriteLine(JsonSerializer.Serialize(tradeResponses, options));
            }
        }

        [Benchmark]
        public async Task RunMultipleTradesWithMaxParallelism()
        {
            string filePath = "Tests/MultipleTrades_Test2.json";
            List<TradeRequest>? tradeRequests = reader.GetTradesFromJson(filePath);
            List<TradeResponse>? tradeResponses = null;
            if (tradeRequests != null)
                tradeResponses = await feeCalculator.CalculateCommissionInParallel(tradeRequests);

            if (tradeResponses != null)
            {
                //System.Console.WriteLine(JsonSerializer.Serialize(tradeResponses, options));
            }
        }

        [Benchmark]
        public async Task RunMultipleTradesInParallel3()
        {
            string filePath = "Tests/MultipleTrades_Test2.json";
            int batchSize = 3;
            await RunInParallel(filePath, batchSize);
        }

        [Benchmark]
        public async Task RunTradesInParallelBatch10()
        {
            string filePath = "Tests/MultipleTrades_Test2.json";
            int batchSize = 10;
            await RunInParallel(filePath, batchSize);
        }

        private async Task RunInParallel(string filePath, int batchSize)
        {
            List<TradeRequest>? tradeRequests = reader.GetTradesFromJson(filePath);
            List<TradeResponse>? tradeResponses = null;
            if (tradeRequests != null)
                tradeResponses = await feeCalculator.CalculateCommissionInParallelBatch(tradeRequests, batchSize);

            if (tradeResponses != null)
            {
                //System.Console.WriteLine(JsonSerializer.Serialize(tradeResponses, options));
            }
        }
    }
}
