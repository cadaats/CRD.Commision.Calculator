using CRD.Commission.Calculator.Models;
using CRD.Commission.Calculator.Models.Enums;

namespace CRD.Commission.Calculator.Interface
{
    public abstract class BaseCalculator
    {
        public abstract SecurityTypes TradeType { get; }
        public abstract Task<TradeResponse> CalculateFee(TradeRequest trade);
    }
}