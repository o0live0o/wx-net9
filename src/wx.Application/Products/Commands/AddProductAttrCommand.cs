namespace wx.Application.Products;

public record AddProductAttrCommand(int ProductId, int CategoryAttrId, string Value) : IRequest<ProductAttributeDto>;