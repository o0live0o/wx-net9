using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Application.Products;

public class UpdateProductAttrCommandHandler(WxContext context) : IRequestHandler<UpdateProductAttrCommand, bool>
{
    public async Task<bool> Handle(UpdateProductAttrCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.Include(p => p.Attributes).SingleOrDefaultAsync(p => p.Id == request.ProductId) ?? throw new KeyNotFoundException();

        product.UpdateAttribute(request.AttrId, request.Value);
        await context.SaveEntitiesAsync();
        return true;
    }
}
