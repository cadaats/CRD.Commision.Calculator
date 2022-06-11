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
    public class TestRunner
    {
        FeeCalculatorService feeCalculator = new FeeCalculatorService();
        JsonReader reader = new JsonReader();

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };

        public async Task RunLinear(string filePath)
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
                    System.Console.WriteLine("Linear Response: " + JsonSerializer.Serialize(tradeResponses, options));
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
                System.Console.WriteLine("Response from Task Parallelism: "+ JsonSerializer.Serialize(tradeResponses, options));
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
                System.Console.WriteLine("Response from Task Parallelism by Partitioning data: " + JsonSerializer.Serialize(tradeResponses, options));
            }
        }
    }
}
