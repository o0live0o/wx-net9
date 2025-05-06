namespace wx.Application.Products;

public record AddProductAttrCommand(int ProductId, string Key, string Value) : IRequest<ProductAttributeDto>;