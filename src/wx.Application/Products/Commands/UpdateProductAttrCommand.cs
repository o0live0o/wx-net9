namespace wx.Application.Products;

public record UpdateProductAttrCommand(int ProductId, int AttrId, string Value) : IRequest<bool>;