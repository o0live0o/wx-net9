using Microsoft.EntityFrameworkCore;
using wx.Application.Products.Commands;
using wx.Infrastructure;

namespace wx.Application.Products;

public class DeleteProductCommandHandler(WxContext context) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.SingleOrDefaultAsync(p => p.Id == request.Id) 
            ?? throw new KeyNotFoundException($"Product {request.Id} not found");

        context.Products.Remove(product);

        await context.SaveEntitiesAsync();

        return true;
    }
}
