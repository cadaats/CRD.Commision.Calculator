using CRD.Commission.Calculator.Interface;
using CRD.Commission.Calculator.Models;
using CRD.Commission.Calculator.Models.Constants;
using CRD.Commission.Calculator.Models.Enums;
using System.ComponentModel.Composition;

namespace CRD.Commission.Calculator.COM
{
    /// <summary>
    /// Commodity fee calc
    /// </summary>
    [Export(typeof(BaseCalculator))]
    public class COMCalculator : BaseCalculator
    {
        public override SecurityTypes TradeType => SecurityTypes.COM;

        public override Task<TradeResponse> CalculateFee(TradeRequest trade)
        {
            TradeResponse response = new TradeResponse(trade);
            response.Commission = (decimal)(TradeConstants.COM_COMMISSION_PCT * trade.TotalPrice) / 100;

            if (trade.TotalPrice > TradeConstants.COM_SELL_ADVISORY_LIMIT && trade.TransactionType == Models.Enums.TransactionType.SELL)
                response.Commission += (decimal)TradeConstants.COM_SELL_ADVISORY_FEE;

            return Task.FromResult(response);
        }
    }
}