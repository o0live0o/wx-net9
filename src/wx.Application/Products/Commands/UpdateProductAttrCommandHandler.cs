namespace wx.Application.Products;

public class UpdateProductAttrCommandHandler(IRepository<Product> repository) : IRequestHandler<UpdateProductAttrCommand, bool>
{
    public async Task<bool> Handle(UpdateProductAttrCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.ProductId) ?? throw new KeyNotFoundException();
        product.UpdateAttribute(request.AttrId, request.Value);
        await repository.UpdateAsync(product);
        return true;
    }
}
