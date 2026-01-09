using wx.Core.Enums;

namespace wx.Core.Domain.Stocks;

public class StockTransaction : Entity
{
    public int UserId { get; set; }
    public string StockCode { get; set; }
    public TransactionSide Side { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Fee { get; set; }
    public decimal CostBasis { get; set; }
    public decimal RealizedPnl { get; set; }
    public DateTime CreatedAt { get; set; }
}
