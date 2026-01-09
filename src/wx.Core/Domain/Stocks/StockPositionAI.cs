using System;
using System.Collections.Generic;
using System.Text;

namespace wx.Core.Domain.Stocks;

public class StockPositionAI : Entity, IAggregateRoot
{
    public required string Symbol { get; init; }
    public int Quantity { get; set; }
    public decimal CostPrice { get; set; }

    private StockPositionAI() { }

    internal static StockPositionAI Create(string symbol, int quantity, decimal costPrice)
    {
        var position = new StockPositionAI()
        {
            Symbol = symbol,
            Quantity = quantity,
            CostPrice = costPrice
        };
        return position;
    }

    internal void UpdateAverageCost(int newQty, decimal newPrice)
    {
        var totalQty = Quantity + newQty;
        CostPrice = (CostPrice * Quantity + newPrice * newQty) / totalQty;
        Quantity = totalQty;
    }

    internal void ReduceQuantity(int qty)
    {
        Quantity -= qty;
    }
}
