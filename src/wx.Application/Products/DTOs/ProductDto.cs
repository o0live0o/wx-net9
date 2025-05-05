namespace wx.Application.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public List<ProductAttributeQueryDto> ProductAttrs { get; set; }
}

public class ProductAttributeQueryDto
{
    public int Id { get; set; }
    public string Name { get; set; }    
    public string Value { get; set; }
}