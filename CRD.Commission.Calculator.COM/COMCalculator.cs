using CRD.Commission.Calculator.Interface;
using CRD.Commission.Calculator.Models;
using CRD.Commission.Calculator.Models.Constants;
using System.ComponentModel.Composition;

namespace CRD.Commission.Calculator.COM
{
    /// <summary>
    /// Commodity fee calc
    /// </summary>
    [Export(typeof(BaseCalculator))]
    public class COMCalculator : BaseCalculator
    {
        public override string TradeType => "COM";

        public override Task<TradeResponse> CalculateFee(Trade trade)
        {
            TradeResponse response = new TradeResponse(trade);
            response.Commission = (decimal)(TradeConstants.COM_COMMISSION_PCT * trade.TotalPrice) / 100;

            if (trade.TotalPrice > TradeConstants.COM_SELL_ADVISORY_LIMIT)
                response.Commission += (decimal)TradeConstants.COM_SELL_ADVISORY_FEE;

            return Task.FromResult(response);
        }
    }
}