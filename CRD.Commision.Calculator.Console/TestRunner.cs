using CRD.Commission.Calculator;
using CRD.Commission.Calculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRD.Commision.Calculator.ConsoleApp
{
    public class TestRunner
    {
        FeeCalculatorService feeCalculator = new FeeCalculatorService();
        JsonReader reader = new JsonReader();

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };

        public async Task RunSingle(TradeRequest tradeRequest)
        {
            if(tradeRequest != null)
            {
                TradeResponse response = await feeCalculator.CalculateCommission(tradeRequest);
                var result = JsonSerializer.Serialize(response, options);
                System.Console.WriteLine("RunSingle Response: " + result);
                File.WriteAllText("../../../Logs/Single.log", result);
            }
        }
        public async Task RunMultipleLinear(string filePath)
        {
            List<TradeRequest>? tradeRequests = reader.GetTradesFromJson(filePath);
            List<TradeResponse>? tradeResponses = new List<TradeResponse>();

            if (tradeRequests != null)
            {
                tradeRequests.ForEach(async t =>
                    {
                        tradeResponses.Add(await feeCalculator.CalculateCommission(t));
                    });

                if (tradeResponses != null)
                {
                    var result = JsonSerializer.Serialize(tradeResponses, options);
                    System.Console.WriteLine("Linear Response: " + result);
                    File.WriteAllText("../../../Logs/Linear.log", result);
                } 
            }
        }

        public async Task RunMultipleTradesWithMaxParallelism(string filePath)
        {
            List<TradeRequest>? tradeRequests = reader.GetTradesFromJson(filePath);
            List<TradeResponse>? tradeResponses = null;
            if (tradeRequests != null)
                tradeResponses = await feeCalculator.CalculateCommissionInParallel(tradeRequests);

            if (tradeResponses != null)
            {
                var result = JsonSerializer.Serialize(tradeResponses, options);
                System.Console.WriteLine("Response from Task Parallelism: "+ result);
                File.WriteAllText("../../../Logs/MaxParallel.log", result);
            }
        }

        public async Task RunMultipleTradesInParallel(string filePath, int batchSize)
        {
            List<TradeRequest>? tradeRequests = reader.GetTradesFromJson(filePath);
            List<TradeResponse>? tradeResponses = null;
            if (tradeRequests != null)
                tradeResponses = await feeCalculator.CalculateCommissionInParallelBatch(tradeRequests, batchSize);

            if (tradeResponses != null)
            {
                var result = JsonSerializer.Serialize(tradeResponses, options);
                System.Console.WriteLine("Response from Task Parallelism by Partitioning data: " + result);
                File.WriteAllText("../../../Logs/Batches.log", result);
            }
        }
    }
}
