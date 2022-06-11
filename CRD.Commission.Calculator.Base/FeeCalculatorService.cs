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
                        string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyNode.InnerText);
                        if (System.IO.File.Exists(file))
                        {
                            ComposablePartCatalog assemblyCatalog = new AssemblyCatalog(Assembly.LoadFile(file));
                            aggCatalog.Catalogs.Add(assemblyCatalog);
                        }
                        else
                            throw new Exception($"Did not find file {assemblyNode.InnerText}");


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

        public Task<TradeResponse> CalculateCommission(Trade trade)
        {
            Task<TradeResponse> tradeResponse;

            var calculator = (from c in AvailableCalculators
                             where c.TradeType.ToLower().Equals(trade.SecurityType.ToLower())
                             select c).FirstOrDefault();

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
    }
}
