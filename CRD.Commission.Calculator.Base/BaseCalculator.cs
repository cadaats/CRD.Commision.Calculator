using CRD.Commission.Calculator.Models;

namespace CRD.Commission.Calculator.Interface
{
    public abstract class BaseCalculator
    {
        public abstract string TradeType { get; }
        public abstract Task<TradeResponse> CalculateFee(TradeRequest trade);
    }
}