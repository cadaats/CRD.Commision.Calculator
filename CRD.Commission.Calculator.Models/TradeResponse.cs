using CRD.Commission.Calculator.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRD.Commission.Calculator.Models
{
    public class TradeResponse
    {
        public DateTime TradeDate { get; } = DateTime.Now;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SecurityTypes SecurityType { get; }
        public double Price { get; } = 0;
        public double Quantity { get; } = 0;
        public decimal? Commission { get; set; }

        public string? ErrorMessage { get; set; }

        public TradeResponse(TradeRequest trade)
        {
            if (trade == null)
                throw new ArgumentNullException("Trade request not found to initialize response.");

            this.TradeDate = trade.TradeDate;
            this.Price = trade.Price;
            this.Quantity = trade.Quantity;
            this.SecurityType = trade.SecurityType;
        }
    }
}
