using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Application.Products;

public class AddProductAttrCommandHandler(WxContext context) : IRequestHandler<AddProductAttrCommand, ProductAttributeDto>
{
    public async Task<ProductAttributeDto> Handle(AddProductAttrCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.Include(p => p.Attributes).SingleOrDefaultAsync(p => p.Id == request.ProductId) ?? 
            throw new KeyNotFoundException();

        var attr = product.SetAttribute(request.Key, request.Value);
        await context.SaveChangesAsync();
        return new ProductAttributeDto() { AttrId = attr.Id, Key  = attr.Key, Value = attr.Value };
    }
}
