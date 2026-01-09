using Microsoft.EntityFrameworkCore;
using wx.Application.Products.Commands;

namespace wx.Application.Products;

public class DeleteProductCommandHandler(IRepository<Product> repository) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id) 
            ?? throw new KeyNotFoundException($"Product {request.Id} not found");

        await repository.DeleteAsync(product);
        return true;
    }
}
