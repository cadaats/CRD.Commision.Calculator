using CRD.Commission.Calculator.Interface;
using CRD.Commission.Calculator.Models;
using CRD.Commission.Calculator.Models.Constants;
using CRD.Commission.Calculator.Models.Enums;
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
        public override SecurityTypes TradeType => SecurityTypes.FX;

        public override Task<TradeResponse> CalculateFee(TradeRequest trade)
        {
            TradeResponse response = new TradeResponse(trade);
            switch (trade.TransactionType)
            {
                case Models.Enums.TransactionType.BUY:
                    response.Commission = (decimal)(TradeConstants.FX_BUY_COMMISSION_PCT * trade.Quantity) / 100;
                    break;
                case Models.Enums.TransactionType.SELL:
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