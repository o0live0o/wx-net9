using System;
using System.Collections.Generic;
using System.Text;

namespace wx.Core.Domain.Stocks;

public class StockAccountAI : Entity, IAggregateRoot
{
    public string AccountName { get; set; }
    public decimal InitCash { get; set; }
    public decimal AvaliableCash { get; set; }

    private List<StockPositionAI> _positions = new List<StockPositionAI>();

    public IReadOnlyCollection<StockPositionAI> Positions => _positions.AsReadOnly();

    private StockAccountAI()
    {
    }

    public void Buy(string symbol, int quantity, decimal price)
    {
        if (AvaliableCash < quantity * price)
            throw new InvalidOperationException("资金不足");

        var existing = _positions.FirstOrDefault(p => p.Symbol == symbol);
        if (existing != null)
        {
            existing.UpdateAverageCost(quantity, price);
        }
        else
        {
            _positions.Add(StockPositionAI.Create(symbol, quantity, price));
        }

    }

    public static StockAccountAI Create(string accountName, decimal initCash)
    {
        var account = new StockAccountAI();
        account.AccountName = accountName;
        account.InitCash = initCash;
        account.AvaliableCash = initCash;
        return account;
    }
}
