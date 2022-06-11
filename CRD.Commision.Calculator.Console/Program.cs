// See https://aka.ms/new-console-template for more information
using CRD.Commission.Calculator;
using CRD.Commission.Calculator.Models;
using System.Text.Json;

Console.WriteLine("Hello  World");

FeeCalculatorService feeCalculator = new FeeCalculatorService();
var comm = await feeCalculator.CalculateCommission(new Trade()
{
    SecurityType = "COM",
    Price = 100,
    Quantity = 10000,
    TransactionType = CRD.Commission.Calculator.Models.Enums.TradeSide.BUY
});
Console.WriteLine(JsonSerializer.Serialize(comm));