using Microsoft.EntityFrameworkCore;
using wx.Infrastructure;

namespace wx.Application.Products;

public class UpdateProductCommandHandler(WxContext context) : IRequestHandler<UpdateProductCommand, bool>
{
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.Include(i => i.Attributes).SingleOrDefaultAsync(p => p.Id == request.Id);
        if (product == null) throw new KeyNotFoundException($"Id: {request.Id} Not Found.");

        ProductBrandVO newBrand = null;
        if (request.Brand != null || request.Model != null)
        {
            newBrand = new ProductBrandVO(
                request.Brand ?? product.BrandModel.Brand,
                request.Model ?? product.BrandModel.Model
            );
        }
        product.Update(
            name: request.Name,
            description: request.Description,
            brand: newBrand
        );

        await context.SaveEntitiesAsync(cancellationToken);
        return true;
    }
}
