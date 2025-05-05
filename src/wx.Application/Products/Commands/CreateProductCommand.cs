namespace wx.Application.Products;

public record CreateProductCommand(string Brand,
                                   string Name,
                                   int CategoryId,
                                   string Model,
                                   string? Description,
                                   IEnumerable<ProductAttributeDto>? Attrs) : IRequest<Product>;