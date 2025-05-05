namespace wx.Application.Products;

public record UploadProductImageCommand(int ProductId, Stream imageStream, string FileName):IRequest<bool>;
