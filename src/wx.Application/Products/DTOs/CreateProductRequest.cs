using Microsoft.AspNetCore.Http;
using wx.Application.Categories;

namespace wx.Application.Products;

public record CreateProductRequest
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string? Description { get; set; }
    public ProductAttributeDto[]? ProductAttrs { get; set; }
    //public IFormFile? Image { get; set; }
}

public class ProductAttributeDto
{
    public int AttrId { get; set; }
    public int CategoryAttributeId { get; set; }
    public string Value { get; set; }

    public static ProductAttributeDto FromEntity(ProductAttribute attr)
    {
        return new ProductAttributeDto()
        {
            AttrId = attr.Id,
            CategoryAttributeId = attr.CategoryAttributeId,
            Value = attr.Value
        };
    }
}