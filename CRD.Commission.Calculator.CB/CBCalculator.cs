﻿using CRD.Commission.Calculator.Interface;
using CRD.Commission.Calculator.Models;
using CRD.Commission.Calculator.Models.Constants;
using System.ComponentModel.Composition;

namespace CRD.Commission.Calculator.CB
{
    /// <summary>
    /// Cash bonds calculator
    /// </summary>
    [Export(typeof(BaseCalculator))]
    public class CBCalculator : BaseCalculator
    {
        public override string TradeType => "CB";

        public override Task<TradeResponse> CalculateFee(TradeRequest trade)
        {
            TradeResponse response = new TradeResponse(trade);

            response.Commission = (decimal)((trade.TransactionType == Models.Enums.TradeSide.BUY) 
                ? TradeConstants.CB_BUY_COMMISSION_PCT 
                : TradeConstants.CB_SELL_COMMISSION_PCT 
                * trade.TotalPrice) 
                / 100;

            return Task.FromResult(response);
        }
    }
}