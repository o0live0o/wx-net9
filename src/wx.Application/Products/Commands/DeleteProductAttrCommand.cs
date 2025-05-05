namespace wx.Application.Products;

public record DeleteProductAttrCommand(int ProductId, int AttrId) : IRequest<bool>;