namespace wx.Core.Entities;

public class ProductBrandVO : ValueObject
{
    public string Brand { get; private set; }

    public string Model { get; private set; }

    public ProductBrandVO()
    {
        
    }

    public ProductBrandVO(string brand, string model)
    {
        Brand = brand;
        Model = model;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Brand;
        yield return Model;
    }
}