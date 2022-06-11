﻿using CRD.Commission.Calculator.Models.Enums;
using System.Text.Json.Serialization;

namespace CRD.Commission.Calculator.Models
{
    public class TradeRequest
    {
        public DateTime TradeDate { get; set; } = DateTime.Now;
        public string SecurityType { get; set; }
        public double Price { get; set; } = 0;
        public double Quantity { get; set; } = 0;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TradeSide TransactionType { get; set; }

        public double TotalPrice => Price * Quantity;
    }
}