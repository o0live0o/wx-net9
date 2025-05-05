namespace wx.Core.Entities;

public class ProductImage : Entity
{
    public int ProductId { get; private set; }

    public Product Product { get; private set; }    

    public string ImageUrl { get; private set; }

    public ProductImage(string imageUrl)
    {
        ImageUrl = imageUrl;
    }

}