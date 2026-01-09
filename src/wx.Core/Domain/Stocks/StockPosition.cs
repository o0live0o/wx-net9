namespace wx.Core.Domain.Stocks;

public class StockPosition : Entity
{
    public string StockCode { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public decimal AvgCostPrice { get; set; }
    public DateTime LastClosedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    private StockPosition() { }

    public void Buy(int quantity, decimal price)
    {
        if(this.Quantity == 0)
            this.AvgCostPrice = price;
        else
        {
            var oldTotalPrice = this.AvgCostPrice * this.Quantity;
            var newTotalPrice = oldTotalPrice + quantity * price;
            this.AvgCostPrice = newTotalPrice / (this.Quantity + quantity);
        }

        this.Quantity += quantity;
    }

    public void Sell(int quantity, decimal price)
    {
        if (quantity <= 0)
            throw new WxException("Sell quantity must be positive");
        if (quantity > this.Quantity)
            throw new WxException("Insufficient shares to sell");

    }
}
