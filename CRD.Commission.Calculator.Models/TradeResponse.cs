using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRD.Commission.Calculator.Models
{
    public class TradeResponse : Trade
    {
        public decimal? Commission { get; set; }

        public string? ErrorMessage { get; set; }

        public TradeResponse(Trade trade)
        {
            this.TradeDate = trade.TradeDate;
            this.Price = trade.Price;
            this.Quantity = trade.Quantity;
            this.SecurityType = trade.SecurityType;
        }
    }
}
