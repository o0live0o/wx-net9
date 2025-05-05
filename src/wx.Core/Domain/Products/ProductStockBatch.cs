namespace wx.Core.Domain.Products;

public class ProductStockBatch : Entity
{
    public string BatchNumber { get; private set; }
    public string Supplier { get; private set; }
    public int Quantity { get; private set; }
    public DateTime EntryDate { get; private set; }

    public ProductStockBatch(string batchNumber, string supplier, int quantity)
    {
        BatchNumber = batchNumber;
        Supplier = supplier;
        Quantity = quantity;
        EntryDate = DateTime.Now;
    }
}