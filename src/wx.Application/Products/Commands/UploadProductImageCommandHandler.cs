using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wx.Infrastructure;
using wx.Shared.Images;

namespace wx.Application.Products;

public class UploadProductImageCommandHandler(IFileService fileService, WxContext context) : IRequestHandler<UploadProductImageCommand, bool>
{
    public async Task<bool> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync(request.ProductId)
            ?? throw new KeyNotFoundException($"Product {request.ProductId} not found");

        string fileName = $"Img{request.ProductId}{Path.GetExtension(request.FileName)}";
        await fileService.SaveImageAsync(request.imageStream, fileName);
        return true;
    }
}
