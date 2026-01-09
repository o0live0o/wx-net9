using wx.Shared.Images;

namespace wx.Application.Products;

public class UploadProductImageCommandHandler(IFileService fileService, IRepository<Product> repository) : IRequestHandler<UploadProductImageCommand, bool>
{
    public async Task<bool> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.ProductId)
            ?? throw new KeyNotFoundException($"Product {request.ProductId} not found");

        string fileName = $"Img{request.ProductId}{Path.GetExtension(request.FileName)}";
        await fileService.SaveImageAsync(request.imageStream, fileName);
        return true;
    }
}
