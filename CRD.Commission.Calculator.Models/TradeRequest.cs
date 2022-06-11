using CRD.Commission.Calculator.Models.Enums;
using System.Text.Json.Serialization;

namespace CRD.Commission.Calculator.Models
{
    public class TradeRequest
    {
        //private DateTime tradeDate;
        //private SecurityTypes securityType;
        //private double price;
        //private double quantity;
        //private TransactionType transactionType;
        public double TotalPrice => Price * Quantity;

        public DateTime TradeDate { get; private set; }
        public double Price { get; private set; }
        public double Quantity { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SecurityTypes SecurityType { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType TransactionType { get; private set; }

        
        [JsonConstructor]
        public TradeRequest(DateTime tradeDate, SecurityTypes securityType, double price, double quantity,  TransactionType transactionType)
        {
            if(securityType == SecurityTypes.None)
                throw new ArgumentException("Please provide a valid security type.");

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price));

            if(transactionType == TransactionType.None)
                throw new ArgumentOutOfRangeException();

            SecurityType = securityType;
            Quantity = quantity;
            Price = price;
            TransactionType = transactionType;
            TradeDate = tradeDate;
        }
    }
}