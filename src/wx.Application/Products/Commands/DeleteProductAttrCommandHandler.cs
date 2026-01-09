namespace wx.Application.Products;

internal class DeleteProductAttrCommandHandler(IRepository<Product> repository) : IRequestHandler<DeleteProductAttrCommand, bool>
{
    public async Task<bool> Handle(DeleteProductAttrCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.ProductId) ?? throw new KeyNotFoundException();
        product.RemoveAttr(request.AttrId);
        await repository.UpdateAsync(product);
        return true;
    }
}