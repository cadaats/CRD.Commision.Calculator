using CRD.Commission.Calculator.Interface;
using CRD.Commission.Calculator.Models;
using CRD.Commission.Calculator.Models.Constants;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;

namespace CRD.Commission.Calculator.FX
{
    /// <summary>
    /// Fx fee calculator
    /// </summary>
    [Export(typeof(BaseCalculator))]
    public class FXCalculator : BaseCalculator
    {
        public override string TradeType => "FX";

        public override Task<TradeResponse> CalculateFee(Trade trade)
        {
            TradeResponse response = new TradeResponse(trade);
            switch (trade.TransactionType)
            {
                case Models.Enums.TradeSide.BUY:
                    response.Commission = (decimal)(TradeConstants.FX_BUY_COMMISSION_PCT * trade.Quantity) / 100;
                    break;
                case Models.Enums.TradeSide.SELL:
                    if (trade.TotalPrice > TradeConstants.FX_SELL_LOWER_LIMIT && trade.TotalPrice <= TradeConstants.FX_SELL_UPPER_LIMIT)
                        response.Commission = (decimal)TradeConstants.FX_SELL_LOWER_COMMISION;
                    else if (trade.TotalPrice > TradeConstants.FX_SELL_UPPER_LIMIT)
                        response.Commission = (decimal)TradeConstants.FX_SELL_UPPER_COMMISION;
                    break;
            }

            return Task.FromResult(response);
        }
    }
}