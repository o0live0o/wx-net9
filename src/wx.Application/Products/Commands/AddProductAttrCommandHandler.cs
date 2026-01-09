using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Application.Products;

public class AddProductAttrCommandHandler(IRepository<Product> repository) : IRequestHandler<AddProductAttrCommand, ProductAttributeDto>
{
    public async Task<ProductAttributeDto> Handle(AddProductAttrCommand request, CancellationToken cancellationToken)
    {
        var product =  await repository.GetByIdAsync(request.ProductId) ?? throw new KeyNotFoundException();
        var attr = product.SetAttribute(request.Key, request.Value);
        await repository.UpdateAsync(product);
        return new ProductAttributeDto() { AttrId = attr.Id, Key  = attr.Key, Value = attr.Value };
    }
}
