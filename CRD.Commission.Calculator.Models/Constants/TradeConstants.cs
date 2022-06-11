using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRD.Commission.Calculator.Models.Constants
{
    /// <summary>
    /// This is populated using class for now, In reality should be pulled from config or database
    /// </summary>
    public sealed record TradeConstants
    {
        #region COM Trades
        public const double COM_COMMISSION_PCT = 0.05;
        public const double COM_SELL_ADVISORY_FEE = 0.05;
        public const double COM_SELL_ADVISORY_LIMIT = 100000;
        #endregion

        #region CB Trades
        public const double CB_BUY_COMMISSION_PCT = 0.02;
        public const double CB_SELL_COMMISSION_PCT = 0.01;
        #endregion

        #region FX Trades
        public const double FX_BUY_COMMISSION_PCT = 0.01;
        public const double FX_SELL_LOWER_LIMIT = 10000;
        public const double FX_SELL_UPPER_LIMIT = 1000000;
        public const double FX_SELL_LOWER_COMMISION = 100;
        public const double FX_SELL_UPPER_COMMISION = 1000;
        #endregion


    }
}
