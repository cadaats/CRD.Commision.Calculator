using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;
using CRD.Commission.Calculator.Interface;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition;
using CRD.Commission.Calculator.Models;
using System.Collections.Concurrent;

namespace CRD.Commission.Calculator
{
    public class FeeCalculatorService
    {
        [ImportMany]
        private IEnumerable<BaseCalculator>? AvailableCalculators { get; set; }
        public FeeCalculatorService()
        {
            try
            {
                AggregateCatalog aggCatalog = new AggregateCatalog();
                ComposablePartCatalog typeCatalog = new TypeCatalog(typeof(BaseCalculator));
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CalculatorAssemblies.xml"));
                XmlNodeList? assemblyNodes = xmldoc.SelectNodes("//Assemblies/Assembly");

                if (assemblyNodes != null)
                {
                    foreach (XmlNode assemblyNode in assemblyNodes)
                    {
                        //string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyNode.InnerText);
                        string file = Path.Combine(@"I:\Console\CRD\CRD.Commision.Calculator\Staging\Release\net6.0\", assemblyNode.InnerText); 
                        if (System.IO.File.Exists(file))
                        {
                            ComposablePartCatalog assemblyCatalog = new AssemblyCatalog(Assembly.LoadFile(file));
                            aggCatalog.Catalogs.Add(assemblyCatalog);
                        }
                        else
                            throw new Exception($"Did not load file: {file}");


                    }
                }

                var container = new CompositionContainer(aggCatalog);
                container.ComposeParts(this);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Calculates commission based on trade type
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public Task<TradeResponse> CalculateCommission(TradeRequest trade)
        {
            Task<TradeResponse> tradeResponse;

            var calculator = (from c in AvailableCalculators
                             where c.TradeType.ToLower().Equals(trade.SecurityType.ToLower())
                             select c).FirstOrDefault();

            Task.Delay(100);

            if(calculator != null)
            {
                tradeResponse = calculator.CalculateFee(trade);
            }
            else
            {
                tradeResponse = Task.FromResult(new TradeResponse(trade) 
                                { 
                                    ErrorMessage = $"Calculator not available for {trade.SecurityType} trade type." 
                                });
            }
            

            return tradeResponse;
        }

        /// <summary>
        /// Overloaded method that processes multiple requests in parallel using partitions
        /// </summary>
        /// <param name="tradeRequests">list of trades for which fee need to be calculated</param>
        /// <returns></returns>
        public Task<List<TradeResponse>> CalculateCommissionInParallelBatch (List<TradeRequest> tradeRequests, int batchSize)
        {
            if(batchSize <= 0)
                batchSize = tradeRequests.Count-1;

            List<TradeResponse> tradeResponses = new List<TradeResponse>();

            Parallel.ForEach(Partitioner.Create(0, tradeRequests.Count, batchSize), async range =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    TradeResponse response = await CalculateCommission(tradeRequests[i]);
                    tradeResponses.Add(response);
                }
            });
            
            return Task.FromResult(tradeResponses);
        }


        public Task<List<TradeResponse>> CalculateCommissionInParallel(List<TradeRequest> tradeRequests)
        {
            List<TradeResponse> tradeResponses = new List<TradeResponse>();

            Parallel.ForEach(tradeRequests, trade=>
            {
                TradeResponse response = CalculateCommission(trade).GetAwaiter().GetResult();
                tradeResponses.Add(response);
            });

            return Task.FromResult(tradeResponses);
        }
    }
}
