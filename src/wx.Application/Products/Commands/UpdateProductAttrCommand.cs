namespace wx.Application.Products;

public record UpdateProductAttrCommand(int ProductId, int CategoryAttrId, string Value) : IRequest<bool>;