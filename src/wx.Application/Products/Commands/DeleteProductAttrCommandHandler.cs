namespace wx.Application.Products;

internal class DeleteProductAttrCommandHandler(WxContext context) : IRequestHandler<DeleteProductAttrCommand, bool>
{
    public async Task<bool> Handle(DeleteProductAttrCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.Include(p => p.Attributes).SingleOrDefaultAsync(p => p.Id == request.ProductId) ?? throw new KeyNotFoundException();
        product.RemoveAttr(request.AttrId);
        await context.SaveChangesAsync();
        return true;
    }
}
