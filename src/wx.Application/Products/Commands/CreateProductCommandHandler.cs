using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wx.Infrastructure;

namespace wx.Application.Products;

public class CreateProductCommandHandler(WxContext context) : IRequestHandler<CreateProductCommand, Product>
{
    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var brand = new ProductBrandVO(request.Brand,request.Model);
        var productNo = Guid.NewGuid().ToString("N2");
        var product = new Product(productNo, request.Name, request.CategoryId, brand, request.Description);

        if (request.Attrs?.Count() > 0)
        {
            foreach (var attr in request.Attrs)
            {
                product.SetAttribute(attr.CategoryAttributeId, attr.Value);
            }
        }

        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }
}
