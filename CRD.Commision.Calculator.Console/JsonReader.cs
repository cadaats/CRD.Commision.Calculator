using CRD.Commission.Calculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRD.Commision.Calculator.Console
{
    internal class JsonReader
    {
        internal List<TradeRequest>? GetTradesFromJson(string filePath)
        {
            List < TradeRequest >? trades;
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                var jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                trades = JsonSerializer.Deserialize<List<TradeRequest>>(json, jsonSerializerOptions);
            }
            return trades;
        }
    }
}
